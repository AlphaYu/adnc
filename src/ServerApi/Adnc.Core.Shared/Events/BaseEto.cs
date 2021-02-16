using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Events
{
    public abstract class BaseEto
    {
        public long Id { get; set; }
        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime OccurredDate { get { return DateTime.Now; } }

        /// <summary>
        /// 触发事件的对象
        /// </summary>
        public object EventSource { get; set; }
    }
}
