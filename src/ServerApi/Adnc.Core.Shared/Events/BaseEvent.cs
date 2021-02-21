using System;

namespace Adnc.Core.Shared.Events
{
    public abstract class BaseEvent
    {
        public BaseEvent(long id, object data)
        {
            this.Id = id;
            this.Data = data;
        }

        /// <summary>
        /// 事件Id
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime OccurredDate { get { return DateTime.Now; } }

        /// <summary>
        /// 触发事件的对象
        /// </summary>
        public string EventSource { get { return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName; } }

        /// <summary>
        /// 事件数据
        /// </summary>
        public object Data { get; protected set; }

    }
}
