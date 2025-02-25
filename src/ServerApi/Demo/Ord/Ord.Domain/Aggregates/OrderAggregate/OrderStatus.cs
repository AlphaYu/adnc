namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

public record OrderStatus : ValueObject
{
    public OrderStatusCodes Code { get; }

    public string? ChangesReason { get; }

    private OrderStatus()
    {
    }

    public OrderStatus(OrderStatusCodes statusCode, string? reason = null)
    {
        Code = statusCode;
        ChangesReason = reason is null ? string.Empty : reason.Trim();
    }
}