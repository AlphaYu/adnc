using Adnc.Infra.EventBus;

namespace Adnc.Demo.Remote.Event;

/// <summary>
/// Customer Recharge Event
/// </summary>
[Serializable]
public class CustomerRechargedEvent : BaseEvent
{
    public CustomerRechargedEvent()
    {
    }

    public CustomerRechargedEvent(long id, string source, long custmerId, decimal amout, long transactionLogId)
        : base(id, source)
    {
        CustomerId = custmerId;
        Amount = amout;
        TransactionLogId = transactionLogId;
    }

    public long CustomerId { get; init; }
    public decimal Amount { get; init; }
    public long TransactionLogId { get; init; }
}
