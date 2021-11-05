using Hangfire.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;

namespace Hangfire.Configuration
{
    /// <summary>
    /// Represents a base class for file based <see cref="IConfigurationProvider"/>.
    /// </summary>
    public abstract class FileConfigurationProvider : IConfigurationProvider, IDisposable
	{
		private const int NumberOfRetries = 3;
		private const int DelayOnRetry = 1000;

		private static readonly ILog _logger = LogProvider.For<FileConfigurationProvider>();
		private FileSystemWatcher _fileWatcher;
		private readonly object _fileWatcherLock = new object();

		/// <summary>
		/// Initializes a new <see cref="IConfigurationProvider"/>
		/// </summary>
		/// <param name="configFile">The source settings file.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public FileConfigurationProvider(string configFile, bool reloadOnChange = true)
			: this(new FileInfo(configFile), reloadOnChange) { }

		/// <summary>
		/// Initializes a new <see cref="IConfigurationProvider"/>
		/// </summary>
		/// <param name="fileInfo">The source settings <see cref="FileInfo"/>.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public FileConfigurationProvider(FileInfo fileInfo, bool reloadOnChange = true)
		{
			if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

			if (!fileInfo.Exists)
				throw new FileNotFoundException($"The json file {fileInfo.FullName} does not exist.");

			ConfigFile = fileInfo;

			ReloadOnChange = reloadOnChange;

			Initialize();
		}
		private void Initialize()
		{
			_fileWatcher = new FileSystemWatcher(ConfigFile.DirectoryName, ConfigFile.Name);
			_fileWatcher.EnableRaisingEvents = ReloadOnChange;
			_fileWatcher.Changed += OnChanged;
			_fileWatcher.Error += OnError;
		}
		private void OnError(object sender, ErrorEventArgs e)
		{
			_logger.InfoException($"File {ConfigFile} occurred errors.", e.GetException());
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			lock (_fileWatcherLock)
			{
				_logger.Info($"File {e.Name} changed, try to reload configuration again...");

				var recurringJobInfos = Load().ToArray();

				if (recurringJobInfos == null || recurringJobInfos.Length == 0) return;

				CronJob.AddOrUpdate(recurringJobInfos);
			}
		}

		/// <summary>
		/// <see cref="RecurringJob"/> configuraion file
		/// </summary>
		public virtual FileInfo ConfigFile { get; set; }

		/// <summary>
		/// Whether the <see cref="RecurringJob"/> should be reloaded.
		/// </summary>
		public virtual bool ReloadOnChange { get; set; }

		/// <summary>
		/// Loads the data for this provider.
		/// </summary>
		/// <returns>The list of <see cref="RecurringJobInfo"/>.</returns>
		public abstract IEnumerable<RecurringJobInfo> Load();

		/// <summary>
		/// Reads from config file.
		/// </summary>
		/// <returns>The string content reading from file.</returns>
		protected virtual string ReadFromFile()
		{
			if (!ConfigFile.Exists)
				throw new FileNotFoundException($"The json file {ConfigFile.FullName} does not exist.");

			var content = string.Empty;

			for (int i = 0; i < NumberOfRetries; ++i)
			{
				try
				{
					// Do stuff with file  
					using (var file = ConfigFile.OpenRead())
					using (StreamReader reader = new StreamReader(file))
						content = reader.ReadToEnd();

					break; // When done we can break loop
				}
				catch (Exception ex) when (
				ex is IOException ||
				ex is SecurityException ||
				ex is UnauthorizedAccessException)
				{
					// Swallow the exception.
					_logger.DebugException($"read file {ConfigFile} error.", ex);

					// You may check error code to filter some exceptions, not every error
					// can be recovered.
					if (i == NumberOfRetries) // Last one, (re)throw exception and exit
						throw;

					Thread.Sleep(DelayOnRetry);
				}
			}

			return content;
		}

		/// <summary>
		/// Disposes the file watcher
		/// </summary>
		public virtual void Dispose()
		{
			_fileWatcher?.Dispose();
		}
	}
}
