namespace Adnc.Shared.Events;

/// <summary>
/// 锁定库存事件
/// </summary>
[Serializable]
public class WarehouseQtyBlockedEvent : EventEntity<WarehouseQtyBlockedEvent.EventData>
{
    public WarehouseQtyBlockedEvent()
    {
    }

    public WarehouseQtyBlockedEvent(long id, EventData eventData, string eventSource)
        : base(id, eventData, eventSource)
    {
    }

    public class EventData
    {
        public long OrderId { get; set; }

        public bool IsSuccess { get; set; }

        public string Remark { get; set; } = string.Empty;
    }
}