namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

public record OrderReceiver : ValueObject
{
    public OrderReceiver(string name, string phone, string address)
    {
        Name = Checker.Variable.NotNullOrWhiteSpace(name, nameof(name));
        Phone = Checker.Variable.NotNullOrWhiteSpace(phone, nameof(phone));
        Address = Checker.Variable.NotNullOrWhiteSpace(address, nameof(address));
    }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Phone
    /// </summary>
    public string Phone { get; }

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; }
}
