using Adnc.Infra.EventBus;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// Lock Inventory Event
/// </summary>
[Serializable]
public class WarehouseQtyBlockedEvent : BaseEvent
{
    public WarehouseQtyBlockedEvent()
    {
    }

    public WarehouseQtyBlockedEvent(long id, string eventSource, long orderId, bool isSuccess, string remark)
        : base(id, eventSource)
    {
        OrderId = orderId;
        IsSuccess = isSuccess;
        Remark = remark;
    }

    public long OrderId { get; set; }

    public bool IsSuccess { get; set; }

    public string Remark { get; set; } = string.Empty;
}
