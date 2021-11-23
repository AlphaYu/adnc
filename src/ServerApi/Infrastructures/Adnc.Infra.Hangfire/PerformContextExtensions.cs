using Hangfire.Common;
using Hangfire.Server;
using System;
using System.Collections.Generic;

namespace Hangfire
{
    /// <summary>
    /// Extensions for <see cref="PerformContext"/>.
    /// </summary>
    public static class PerformContextExtensions
    {
        /// <summary>
        /// Gets job data from storage associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.JobData"/></param>
        /// <returns>The value from the property <see cref="RecurringJobInfo.JobData"/></returns>
        public static object GetJobData(this PerformContext context, string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var jobData = GetJobData(context);

            if (jobData == null) return null;

            return jobData.ContainsKey(name) ? jobData[name] : null;
        }

        /// <summary>
        /// Gets job data from storage associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <typeparam name="T">The specified type to json value.</typeparam>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.JobData"/></param>
        /// <returns>The value from the property <see cref="RecurringJobInfo.JobData"/></returns>
        public static T GetJobData<T>(this PerformContext context, string name)
        {
            var o = GetJobData(context, name);

            var json = SerializationHelper.Serialize(o);

            return SerializationHelper.Deserialize<T>(json);
        }

        /// <summary>
        /// Gets job data from storage associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <returns>The job data from storage.</returns>
        public static IDictionary<string, object> GetJobData(this PerformContext context)
        {
            using var storage = new RecurringJobInfoStorage(context.Connection);
            return storage.FindByJobId(context.BackgroundJob.Id)?.JobData;
        }

        /// <summary>
        /// Persists job data to storage associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="name">The dictionary key from the property <see cref="RecurringJobInfo.JobData"/></param>
        /// <param name="value">The persisting value.</param>
        public static void SetJobData(this PerformContext context, string name, object value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            SetJobData(context, new Dictionary<string, object> { [name] = value });
        }

        /// <summary>
        /// Persists job data to storage associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="context">The <see cref="PerformContext"/>.</param>
        /// <param name="jobData">The dictionary value to be added or updated. </param>
        public static void SetJobData(this PerformContext context, IDictionary<string, object> jobData)
        {
            if (jobData == null) throw new ArgumentNullException(nameof(jobData));

            using var storage = new RecurringJobInfoStorage(context.Connection);
            var recurringJobInfo = storage.FindByJobId(context.BackgroundJob.Id);

            if (recurringJobInfo.JobData == null)
                recurringJobInfo.JobData = new Dictionary<string, object>();

            foreach (var kv in jobData)
                recurringJobInfo.JobData[kv.Key] = kv.Value;

            storage.SetJobData(recurringJobInfo);
        }
    }
}