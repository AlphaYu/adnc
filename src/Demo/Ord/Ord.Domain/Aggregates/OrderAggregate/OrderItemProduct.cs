namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

public record OrderItemProduct : ValueObject
{
    public long Id { get; }

    public string Name { get; } 

    public decimal Price { get; }

    private OrderItemProduct()
    {
        Name = string.Empty;
    }

    public OrderItemProduct(long id, string name, decimal price)
    {
        Id = Checker.GTZero(id, nameof(id));
        Name = Checker.NotNullOrEmpty(name, nameof(name));
        Price = Checker.GTZero(price, nameof(price));
    }
}