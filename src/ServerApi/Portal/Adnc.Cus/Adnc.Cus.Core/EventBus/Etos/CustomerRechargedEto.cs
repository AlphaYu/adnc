using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Core.Shared.Events;

namespace Adnc.Cus.Core.EventBus.Etos
{
    public class CustomerRechargedEto
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 事件发生的时间
        /// </summary>
        public DateTime OccurredDate { get { return DateTime.Now; } }

        public string EventSource { get; set; }

        public decimal Amount { get; set; }

        public long TransactionLogId { get; set; }
    }
}
