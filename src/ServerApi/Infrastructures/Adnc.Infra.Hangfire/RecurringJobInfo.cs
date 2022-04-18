using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hangfire
{
    /// <summary>
    /// It is used to build <see cref="RecurringJob"/>
    /// with <see cref="IRecurringJobBuilder.Build(Func{System.Collections.Generic.IEnumerable{RecurringJobInfo}})"/>.
    /// </summary>
    public class RecurringJobInfo
    {
        /// <summary>
        /// The identifier of the RecurringJob
        /// </summary>
        public string RecurringJobId { get; set; }

        /// <summary>
        /// Cron expressions
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// TimeZoneInfo
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// Queue name
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// Method to execute while <see cref="RecurringJob"/> running.
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// The <see cref="RecurringJob"/> data persisted in storage.
        /// </summary>
        public IDictionary<string, object> JobData { get; set; }

        /// <summary>
        /// Whether the <see cref="RecurringJob"/> can be added/updated,
        /// default value is true, if false it will be deleted automatically.
        /// </summary>
        public bool Enable { get; set; }
    }
}