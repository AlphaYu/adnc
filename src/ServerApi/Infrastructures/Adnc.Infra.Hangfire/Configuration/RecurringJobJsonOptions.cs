using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Hangfire.Configuration
{
    /// <summary>
    /// <see cref="RecurringJob"/> json configuration settings.
    /// </summary>
    public class RecurringJobJsonOptions
	{
		/// <summary>
		/// The job name represents for <see cref="RecurringJobInfo.RecurringJobId"/>
		/// </summary>
		[JsonProperty("job-name")]
#if !NET45
		[JsonRequired]
#endif
		public string JobName { get; set; }
		/// <summary>
		/// The job type while impl the interface <see cref="IRecurringJob"/>.
		/// </summary>
		[JsonProperty("job-type")]
#if !NET45
		[JsonRequired]
#endif
		public Type JobType { get; set; }

		/// <summary>
		/// Cron expressions
		/// </summary>
		[JsonProperty("cron-expression")]
#if !NET45
		[JsonRequired]
#endif
		public string Cron { get; set; }

		/// <summary>  
		/// The value of <see cref="TimeZoneInfo"/> can be created by <seealso cref="TimeZoneInfo.FindSystemTimeZoneById(string)"/>
		/// </summary>
		[JsonProperty("timezone")]
		[JsonConverter(typeof(TimeZoneInfoConverter))]
		public TimeZoneInfo TimeZone { get; set; }
		/// <summary>
		/// Whether the property <see cref="TimeZone"/> can be serialized or not.
		/// </summary>
		/// <returns>true if value not null, otherwise false.</returns>
		public bool ShouldSerializeTimeZone() => TimeZone != null;
		/// <summary>
		/// Hangfire queue name
		/// </summary>
		[JsonProperty("queue")]
		public string Queue { get; set; }
		/// <summary>
		/// Whether the property <see cref="Queue"/> can be serialized or not.
		/// </summary>
		/// <returns>true if value not null or empty, otherwise false.</returns>
		public bool ShouldSerializeQueue() => !string.IsNullOrEmpty(Queue);
		/// <summary>
		/// The <see cref="RecurringJob"/> data persisted in storage.  
		/// </summary>
		[JsonProperty("job-data")]
		public IDictionary<string, object> JobData { get; set; }
		/// <summary>
		/// Whether the property <see cref="JobData"/> can be serialized or not.
		/// </summary>
		/// <returns>true if value not null or count is zero, otherwise false.</returns>
		public bool ShouldSerializeJobData() => JobData != null && JobData.Count > 0;

		/// <summary>
		/// Whether the <see cref="RecurringJob"/> can be added/updated,
		/// default value is true, if false it will be deleted automatically.
		/// </summary>
		[JsonProperty("enable")]
		public bool? Enable { get; set; }

		/// <summary>
		/// Whether the property <see cref="Enable"/> can be serialized or not.
		/// </summary>
		/// <returns>true if value is not null, otherwise false.</returns>
		public bool ShouldSerializeEnable() => Enable.HasValue;
	}
}
