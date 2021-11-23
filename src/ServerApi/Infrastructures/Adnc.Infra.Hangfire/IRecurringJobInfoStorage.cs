using System;
using System.Collections.Generic;

namespace Hangfire
{
    /// <summary>
    /// The storage APIs for <see cref="RecurringJobInfo"/>.
    /// </summary>
    public interface IRecurringJobInfoStorage : IDisposable
    {
        /// <summary>
        /// Finds all <see cref="RecurringJobInfo"/> from storage.
        /// </summary>
        /// <returns>The collection of <see cref="RecurringJobInfo"/></returns>
        IEnumerable<RecurringJobInfo> FindAll();

        /// <summary>
        /// Finds <see cref="RecurringJobInfo"/> by jobId.
        /// The job id is associated with <seealso cref="BackgroundJob.Id"/>
        /// </summary>
        /// <param name="jobId">The specified <see cref="BackgroundJob.Id"/></param>
        /// <returns><see cref="RecurringJobInfo"/></returns>
        RecurringJobInfo FindByJobId(string jobId);

        /// <summary>
        /// Finds <see cref="RecurringJobInfo"/> by recurringJobId.
        /// </summary>
        /// <param name="recurringJobId">The specified identifier of the RecurringJob.</param>
        /// <returns><see cref="RecurringJobInfo"/></returns>
        RecurringJobInfo FindByRecurringJobId(string recurringJobId);

        /// <summary>
        /// Sets <see cref="RecurringJobInfo"/> to storage which associated with <see cref="RecurringJob"/>.
        /// </summary>
        /// <param name="recurringJobInfo">The specified identifier of the RecurringJob.</param>
        void SetJobData(RecurringJobInfo recurringJobInfo);
    }
}