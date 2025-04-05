namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

public record OrderStatus : ValueObject
{
    private OrderStatus()
    {
    }

    public OrderStatus(OrderStatusCodes statusCode, string? reason = null)
    {
        Code = statusCode;
        ChangesReason = reason is null ? string.Empty : reason.Trim();
    }

    public OrderStatusCodes Code { get; }

    public string? ChangesReason { get; }
}
