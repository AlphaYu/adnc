using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Hangfire
{
    /// <summary>
    /// Build <see cref="RecurringJob"/> automatically, <see cref="IRecurringJobBuilder"/> interface.
    /// </summary>
    public class RecurringJobBuilder : IRecurringJobBuilder
    {
        private IRecurringJobRegistry _registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobBuilder"/>.
        /// </summary>
        public RecurringJobBuilder() : this(new RecurringJobRegistry()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecurringJobBuilder"/>	with <see cref="IRecurringJobRegistry"/>.
        /// </summary>
        /// <param name="registry"><see cref="IRecurringJobRegistry"/> interface.</param>
        public RecurringJobBuilder(IRecurringJobRegistry registry)
        {
            _registry = registry;
        }
        /// <summary>
        /// Create <see cref="RecurringJob"/> with the provider for specified interface or class.
        /// </summary>
        /// <param name="typesProvider">Specified interface or class</param>
        public void Build(Func<IEnumerable<Type>> typesProvider)
        {
            if (typesProvider == null) throw new ArgumentNullException(nameof(typesProvider));

            foreach (var type in typesProvider())
            {
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    if (!method.IsDefined(typeof(RecurringJobAttribute), false)) continue;

                    var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);

                    if (attribute == null) continue;

                    if (string.IsNullOrWhiteSpace(attribute.RecurringJobId))
                    {
                        attribute.RecurringJobId = method.GetRecurringJobId();
                    }

                    if (!attribute.Enabled)
                    {
                        RecurringJob.RemoveIfExists(attribute.RecurringJobId);
                        continue;
                    }
                    _registry.Register(
                        attribute.RecurringJobId,
                        method,
                        attribute.Cron,
                        attribute.TimeZone,
                        attribute.Queue ?? EnqueuedState.DefaultQueue);
                }
            }
        }
        /// <summary>
        /// Create <see cref="RecurringJob"/> with the provider for specified list <see cref="RecurringJobInfo"/>.
        /// </summary>
        /// <param name="recurringJobInfoProvider">The provider to get <see cref="RecurringJobInfo"/> list.</param>
        public void Build(Func<IEnumerable<RecurringJobInfo>> recurringJobInfoProvider)
        {
            if (recurringJobInfoProvider == null) throw new ArgumentNullException(nameof(recurringJobInfoProvider));

            foreach (RecurringJobInfo recurringJobInfo in recurringJobInfoProvider())
            {
                if (string.IsNullOrWhiteSpace(recurringJobInfo.RecurringJobId))
                {
                    throw new Exception($"The property of {nameof(recurringJobInfo.RecurringJobId)} is null, empty, or consists only of white-space.");
                }
                if (!recurringJobInfo.Enable)
                {
                    RecurringJob.RemoveIfExists(recurringJobInfo.RecurringJobId);
                    continue;
                }
                _registry.Register(recurringJobInfo);
            }
        }
    }
}
