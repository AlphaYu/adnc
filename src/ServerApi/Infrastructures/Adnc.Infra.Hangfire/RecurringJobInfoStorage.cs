using Hangfire.Common;
using Hangfire.Storage;
using System;
using System.Collections.Generic;

namespace Hangfire
{
    /// <summary>
    /// The storage APIs for <see cref="RecurringJobInfo"/>.
    /// </summary>
    public class RecurringJobInfoStorage : IRecurringJobInfoStorage
    {
        private static readonly TimeSpan LockTimeout = TimeSpan.FromMinutes(1);

        private readonly IStorageConnection _connection;

        /// <summary>
        /// Initializes a new <see cref="RecurringJobInfoStorage"/>
        /// </summary>
        public RecurringJobInfoStorage() : this(JobStorage.Current.GetConnection()) { }

        /// <summary>
        /// Initializes a new <see cref="RecurringJobInfoStorage"/>
        /// </summary>
        /// <param name="connection"><see cref="IStorageConnection"/></param>
        public RecurringJobInfoStorage(IStorageConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Finds all <see cref="RecurringJobInfo"/> from storage.
        /// </summary>
        /// <returns>The collection of <see cref="RecurringJobInfo"/></returns>
        public IEnumerable<RecurringJobInfo> FindAll()
        {
            var recurringJobIds = _connection.GetAllItemsFromSet("recurring-jobs");

            foreach (var recurringJobId in recurringJobIds)
            {
                var recurringJob = _connection.GetAllEntriesFromHash($"recurring-job:{recurringJobId}");

                if (recurringJob == null) continue;

                yield return InternalFind(recurringJobId, recurringJob);
            }
        }

        /// <summary>
        /// Finds <see cref="RecurringJobInfo"/> by jobId.
        /// The job id is associated with <seealso cref="BackgroundJob.Id"/>
        /// </summary>
        /// <param name="jobId">The specified <see cref="BackgroundJob.Id"/></param>
        /// <returns><see cref="RecurringJobInfo"/></returns>
        public RecurringJobInfo FindByJobId(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) throw new ArgumentNullException(nameof(jobId));

            var paramValue = _connection.GetJobParameter(jobId, "RecurringJobId");

            if (string.IsNullOrEmpty(paramValue)) throw new ArgumentException($"There is not RecurringJobId with associated BackgroundJob Id:{jobId}");

            var recurringJobId = SerializationHelper.Serialize<string>(paramValue);

            return FindByRecurringJobId(recurringJobId);
        }

        /// <summary>
        /// Finds <see cref="RecurringJobInfo"/> by recurringJobId.
        /// </summary>
        /// <param name="recurringJobId">The specified identifier of the RecurringJob.</param>
        /// <returns><see cref="RecurringJobInfo"/></returns>
        public RecurringJobInfo FindByRecurringJobId(string recurringJobId)
        {
            if (string.IsNullOrEmpty(recurringJobId)) throw new ArgumentNullException(nameof(recurringJobId));

            var recurringJob = _connection.GetAllEntriesFromHash($"recurring-job:{recurringJobId}");

            if (recurringJob == null) return null;

            return InternalFind(recurringJobId, recurringJob);
        }

        private RecurringJobInfo InternalFind(string recurringJobId, Dictionary<string, string> recurringJob)
        {
            if (string.IsNullOrEmpty(recurringJobId)) throw new ArgumentNullException(nameof(recurringJobId));
            if (recurringJob == null) throw new ArgumentNullException(nameof(recurringJob));

            var serializedJob = SerializationHelper.Deserialize<InvocationData>(recurringJob["Job"]);
            var job = serializedJob.DeserializeJob();

            return new RecurringJobInfo
            {
                RecurringJobId = recurringJobId,
                Cron = recurringJob["Cron"],
                TimeZone = recurringJob.ContainsKey("TimeZoneId")
                    ? TimeZoneInfo.FindSystemTimeZoneById(recurringJob["TimeZoneId"])
                    : TimeZoneInfo.Utc,
                Queue = recurringJob["Queue"],
                Method = job.Method,
                Enable = !recurringJob.ContainsKey(nameof(RecurringJobInfo.Enable))
                         || SerializationHelper.Deserialize<bool>(recurringJob[nameof(RecurringJobInfo.Enable)]),
                JobData = recurringJob.ContainsKey(nameof(RecurringJobInfo.JobData))
                    ? SerializationHelper.Deserialize<Dictionary<string, object>>(recurringJob[nameof(RecurringJobInfo.JobData)])
                    : null
            };
        }

        /// <summary>
        /// Sets <see cref="RecurringJobInfo"/> to storage which associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="recurringJobInfo">The specified identifier of the RecurringJob.</param>
        public void SetJobData(RecurringJobInfo recurringJobInfo)
        {
            if (recurringJobInfo == null) throw new ArgumentNullException(nameof(recurringJobInfo));

            if (recurringJobInfo.JobData == null || recurringJobInfo.JobData.Count == 0) return;

            using (_connection.AcquireDistributedLock($"recurringjobextensions-jobdata:{recurringJobInfo.RecurringJobId}", LockTimeout))
            {
                var changedFields = new Dictionary<string, string>
                {
                    [nameof(RecurringJobInfo.Enable)] = SerializationHelper.Serialize(recurringJobInfo.Enable),
                    [nameof(RecurringJobInfo.JobData)] = SerializationHelper.Serialize(recurringJobInfo.JobData)
                };

                _connection.SetRangeInHash($"recurring-job:{recurringJobInfo.RecurringJobId}", changedFields);
            }
        }

        /// <summary>
        /// Disposes storage connection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _connection?.Dispose();
        }
    }
}