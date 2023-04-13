namespace Adnc.Shared.Rpc.Event;

/// <summary>
/// 订单支付事件
/// </summary>
[Serializable]
public sealed class OrderPaidEvent : EventEntity
{
    public OrderPaidEvent()
    {
    }

    public OrderPaidEvent(long id, string eventSource, long orderId, long custmerId, decimal amout)
        : base(id, eventSource)
    {
        OrderId = orderId;
        CustomerId = custmerId;
        Amount = amout;
    }

    public long OrderId { get; init; }

    public long CustomerId { get; init; }

    public decimal Amount { get; init; }
}