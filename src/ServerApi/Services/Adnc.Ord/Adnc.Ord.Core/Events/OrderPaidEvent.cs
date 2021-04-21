using Adnc.Infra.EventBus;

namespace Adnc.Ord.Core.Events
{
    /// <summary>
    /// 订单支付事件
    /// </summary>
    public sealed class OrderPaidEvent : BaseEvent<OrderPaidEvent.EventData>
    {
        public OrderPaidEvent(long id, EventData eventData, string eventSource)
            : base(id, eventData, eventSource)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }

            public long CustomerId { get; set; }

            public decimal Amount { get; set; }
        }
    }
}
