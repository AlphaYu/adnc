using Hangfire.Common;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hangfire.Configuration
{
    /// <summary>
    /// Represents a JSON file provider as an <see cref="IConfigurationProvider"/>.
    /// </summary>
    public class JsonConfigurationProvider : FileConfigurationProvider
	{
		/// <summary>
		/// Initializes a new <see cref="JsonConfigurationProvider"/>.
		/// </summary>
		/// <param name="configFile">The source settings file.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public JsonConfigurationProvider(string configFile, bool reloadOnChange = true)
			: base(configFile, reloadOnChange) { }

		/// <summary>
		/// Loads the <see cref="RecurringJobInfo"/> for this source.
		/// </summary>
		/// <returns>The list of <see cref="RecurringJobInfo"/> for this provider.</returns>
		public override IEnumerable<RecurringJobInfo> Load()
		{
			var jsonContent = ReadFromFile();

			if (string.IsNullOrWhiteSpace(jsonContent)) throw new Exception("Json file content is empty.");

			var jsonOptions = JobHelper.FromJson<List<RecurringJobJsonOptions>>(jsonContent);

			foreach (var o in jsonOptions)
				yield return Convert(o);
		}

		private RecurringJobInfo Convert(RecurringJobJsonOptions option)
		{
			ValidateJsonOptions(option);

			return new RecurringJobInfo
			{
				RecurringJobId = option.JobName,
#if NET45
				Method = option.JobType.GetMethod(nameof(IRecurringJob.Execute)),
#else
				Method = option.JobType.GetTypeInfo().GetDeclaredMethod(nameof(IRecurringJob.Execute)),
#endif
				Cron = option.Cron,
				Queue = option.Queue ?? EnqueuedState.DefaultQueue,
				TimeZone = option.TimeZone ?? TimeZoneInfo.Utc,
				JobData = option.JobData,
				Enable = option.Enable ?? true
			};
		}

		private void ValidateJsonOptions(RecurringJobJsonOptions option)
		{
			if (option == null) throw new ArgumentNullException(nameof(option));

			if (string.IsNullOrWhiteSpace(option.JobName))
			{
				throw new Exception($"The json token 'job-name' is null, empty, or consists only of white-space.");
			}

			if (!option.JobType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IRecurringJob)))
			{
				throw new Exception($"job-type: {option.JobType} must impl the interface {typeof(IRecurringJob)}.");
			}

		}
	}
}
