namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

public record OrderReceiver : ValueObject
{
    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; }

    /// <summary>
    /// 地址
    /// </summary>
    public string Address { get; }

    public OrderReceiver(string name, string phone, string address)
    {
        this.Name = Guard.Checker.NotNullOrEmpty(name, nameof(name));
        this.Phone = Guard.Checker.NotNullOrEmpty(phone, nameof(phone));
        this.Address = Guard.Checker.NotNullOrEmpty(address, nameof(address));
    }
}