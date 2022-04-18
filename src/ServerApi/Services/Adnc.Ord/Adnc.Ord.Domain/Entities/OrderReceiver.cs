﻿namespace Adnc.Ord.Domain.Entities;

public class OrderReceiver : ValueObject
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
        this.Name = Checker.NotNullOrEmpty(name, nameof(name));
        this.Phone = Checker.NotNullOrEmpty(phone, nameof(phone));
        this.Address = Checker.NotNullOrEmpty(address, nameof(address));
    }
}