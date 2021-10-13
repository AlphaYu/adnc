using Hangfire.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hangfire
{
    /// <summary>
    /// The helper class to build <see cref="RecurringJob"/> automatically.
    /// </summary>
    public class CronJob
	{
		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// </summary>
		/// <param name="types">Specified interface or class</param>
		public static void AddOrUpdate(params Type[] types)
		{
			AddOrUpdate(() => types);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically within specified interface or class.
		/// </summary>
		/// <param name="typesProvider">The provider to get specified interfaces or class.</param>
		public static void AddOrUpdate(Func<IEnumerable<Type>> typesProvider)
		{
			if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

			IRecurringJobBuilder builder = new RecurringJobBuilder();

			builder.Build(typesProvider);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically by using multiple JSON configuration files.
		/// </summary>
		/// <param name="jsonFiles">The array of json files.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public static void AddOrUpdate(string[] jsonFiles, bool reloadOnChange = true)
		{
			if (jsonFiles == null) throw new ArgumentNullException(nameof(jsonFiles));

			foreach (var jsonFile in jsonFiles)
				AddOrUpdate(jsonFile, reloadOnChange);
		}
		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically by using a JSON configuration.
		/// </summary>
		/// <param name="jsonFile">Json file for <see cref="RecurringJob"/> configuration.</param>
		/// <param name="reloadOnChange">Whether the <see cref="RecurringJob"/> should be reloaded if the file changes.</param>
		public static void AddOrUpdate(string jsonFile, bool reloadOnChange = true)
		{
			if (string.IsNullOrWhiteSpace(jsonFile)) throw new ArgumentNullException(nameof(jsonFile));

			var configFile = File.Exists(jsonFile) ? jsonFile :
				Path.Combine(
#if NET45
				AppDomain.CurrentDomain.BaseDirectory,
#else
				AppContext.BaseDirectory,
#endif
				jsonFile);


			if (!File.Exists(configFile)) throw new FileNotFoundException($"The json file {configFile} does not exist.");

			IConfigurationProvider provider = new JsonConfigurationProvider(configFile, reloadOnChange);

			AddOrUpdate(provider);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically with <seealso cref="IConfigurationProvider"/>.
		/// </summary>
		/// <param name="provider"><see cref="IConfigurationProvider"/></param>
		public static void AddOrUpdate(IConfigurationProvider provider)
		{
			if (provider == null) throw new ArgumentNullException(nameof(provider));

			IRecurringJobBuilder builder = new RecurringJobBuilder();

			AddOrUpdate(provider.Load());
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically with the collection of <seealso cref="RecurringJobInfo"/>.
		/// </summary>
		/// <param name="recurringJobInfos">The collection of <see cref="RecurringJobInfo"/>.</param>
		public static void AddOrUpdate(IEnumerable<RecurringJobInfo> recurringJobInfos)
		{
			if (recurringJobInfos == null) throw new ArgumentNullException(nameof(recurringJobInfos));

			IRecurringJobBuilder builder = new RecurringJobBuilder();

			builder.Build(() => recurringJobInfos);
		}

		/// <summary>
		/// Builds <see cref="RecurringJob"/> automatically with the array of <seealso cref="RecurringJobInfo"/>.
		/// </summary>
		/// <param name="recurringJobInfos">The array of <see cref="RecurringJobInfo"/>.</param>
		public static void AddOrUpdate(params RecurringJobInfo[] recurringJobInfos)
		{
			if (recurringJobInfos == null) throw new ArgumentNullException(nameof(recurringJobInfos));

			AddOrUpdate(recurringJobInfos.AsEnumerable());
		}
	}
}
