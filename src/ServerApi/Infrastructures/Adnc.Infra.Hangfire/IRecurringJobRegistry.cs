using System;
using System.Reflection;

namespace Hangfire
{
    /// <summary>
    /// Register <see cref="RecurringJob"/> dynamically.
    /// </summary>
    public interface IRecurringJobRegistry
    {
        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        void Register(MethodInfo method, string cron, TimeZoneInfo timeZone, string queue);

        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="recurringJobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue);

        /// <summary>
        /// Register RecurringJob via <see cref="RecurringJobInfo"/>.
        /// </summary>
        /// <param name="recurringJobInfo"><see cref="RecurringJob"/> info.</param>
        void Register(RecurringJobInfo recurringJobInfo);
    }
}