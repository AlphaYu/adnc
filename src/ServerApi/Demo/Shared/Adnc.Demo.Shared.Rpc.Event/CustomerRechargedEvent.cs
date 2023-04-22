namespace Adnc.Shared.Rpc.Event;

/// <summary>
/// 客户充值事件
/// </summary>
[Serializable]
public class CustomerRechargedEvent : EventEntity
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