using Adnc.Infr.EventBus;

namespace Adnc.Whse.Core.Events
{
    /// <summary>
    /// 锁定库存事件
    /// </summary>
    public class WarehouseQtyBlockedEvent : BaseEvent<WarehouseQtyBlockedEvent.EventData>
    {
        public WarehouseQtyBlockedEvent(long id, EventData eventData, string eventSource)
            : base(id, eventData, eventSource)
        {
        }

        public class EventData
        {
            public long OrderId { get; set; }

            public bool IsSuccess{ get; set; }

            public string Remark { get; set; }
        }
    }
}
