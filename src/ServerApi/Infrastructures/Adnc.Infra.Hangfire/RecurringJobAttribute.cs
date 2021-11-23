using Hangfire.States;
using System;

namespace Hangfire
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RecurringJobAttribute : Attribute
    {
        /// <summary>
        /// RecurringJob 的标识
        /// </summary>
        public string RecurringJobId { get; set; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Queue { get; set; } = EnqueuedState.DefaultQueue;

        /// <summary>
        /// 时区信息,默认值为 <see cref="TimeZoneInfo.Utc"/>
        /// </summary>
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Utc;

        /// <summary>
        /// 是否自动构建RecurringJob，默认为true,为false，它将被自动删除。
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}