using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Hangfire
{
    /// <summary>
    /// Register <see cref="RecurringJob"/> dynamically.
    /// <see cref="IRecurringJobRegistry"/> interface.
    /// </summary>
    public class RecurringJobRegistry : IRecurringJobRegistry
    {
        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method">The specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        public void Register(MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var jobId = method.GetRecurringJobId();

            Register(jobId, method, cron, timeZone, queue);
        }

        /// <summary>
        /// Register RecurringJob via <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="recurringJobId">The identifier of the RecurringJob</param>
        /// <param name="method">the specified method</param>
        /// <param name="cron">Cron expressions</param>
        /// <param name="timeZone"><see cref="TimeZoneInfo"/></param>
        /// <param name="queue">Queue name</param>
        public void Register(string recurringJobId, MethodInfo method, string cron, TimeZoneInfo timeZone, string queue)
        {
            if (recurringJobId == null) throw new ArgumentNullException(nameof(recurringJobId));
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (cron == null) throw new ArgumentNullException(nameof(cron));
            if (timeZone == null) throw new ArgumentNullException(nameof(timeZone));
            if (queue == null) throw new ArgumentNullException(nameof(queue));

            var parameters = method.GetParameters();

            Expression[] args = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");

            var methodCall = method.IsStatic ? Expression.Call(method, args) : Expression.Call(x, method, args);

            var addOrUpdate = Expression.Call(
                typeof(RecurringJob),
                nameof(RecurringJob.AddOrUpdate),
                new Type[] { method.DeclaringType },
                new Expression[]
                {
                    Expression.Constant(recurringJobId),
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(cron),
                    Expression.Constant(timeZone),
                    Expression.Constant(queue)
                });

            Expression.Lambda(addOrUpdate).Compile().DynamicInvoke();
        }

        /// <summary>
        /// Register RecurringJob via <see cref="RecurringJobInfo"/>.
        /// </summary>
        /// <param name="recurringJobInfo"><see cref="RecurringJob"/> info.</param>
        public void Register(RecurringJobInfo recurringJobInfo)
        {
            if (recurringJobInfo == null) throw new ArgumentNullException(nameof(recurringJobInfo));

            Register(recurringJobInfo.RecurringJobId, recurringJobInfo.Method, recurringJobInfo.Cron, recurringJobInfo.TimeZone, recurringJobInfo.Queue);

            using (var storage = new RecurringJobInfoStorage())
            {
                storage.SetJobData(recurringJobInfo);
            }
        }
    }
}