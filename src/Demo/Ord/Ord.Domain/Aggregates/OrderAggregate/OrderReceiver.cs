﻿namespace Adnc.Demo.Ord.Domain.Aggregates.OrderAggregate;

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
        Name = Checker.Variable.NotNullOrWhiteSpace(name, nameof(name));
        Phone = Checker.Variable.NotNullOrWhiteSpace(phone, nameof(phone));
        Address = Checker.Variable.NotNullOrWhiteSpace(address, nameof(address));
    }
}