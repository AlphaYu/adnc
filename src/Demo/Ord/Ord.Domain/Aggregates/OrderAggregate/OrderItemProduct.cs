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
        Id = Checker.Variable.GTZero(id, nameof(id));
        Name = Checker.Variable.NotNullOrWhiteSpace(name, nameof(name));
        Price = Checker.Variable.GTZero(price, nameof(price));
    }
}