﻿namespace Adnc.Ord.Domain.Entities;

/// <summary>
/// 订单
/// </summary>
public class Order : AggregateRootWithBasicAuditInfo
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public long CustomerId { get; private set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal Amount { private set; get; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// 收货信息
    /// </summary>
    public OrderReceiver Receiver { get; private set; }

    /// <summary>
    /// 订单子项
    /// </summary>
    public virtual ICollection<OrderItem> Items { get; private set; }

    private Order()
    {
    }

    internal Order(long id, long customerId, OrderReceiver orderReceiver, string? remark = null)
    {
        this.Id = Checker.GTZero(id, nameof(id));
        this.CustomerId = Checker.GTZero(customerId, nameof(customerId));
        this.Items = new List<OrderItem>();
        this.Receiver = Checker.NotNull(orderReceiver, nameof(orderReceiver));
        this.Status = new OrderStatus(OrderStatusEnum.Creating);
        this.Remark = remark;
        this.Amount = 0;
    }

    /// <summary>
    /// 添加订单产品
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="product"></param>
    /// <param name="count"></param>
    public void AddProduct(long itemId, OrderItemProduct product, int count)
    {
        Checker.NotNull(product, nameof(product));
        Checker.GTZero(count, nameof(count));

        var existProduct = this.Items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (existProduct == null)
        {
            this.Items.Add(new OrderItem(itemId, this.Id, product, count));
        }
        else
        {
            existProduct.ChangeCount(count);
        }
        this.Amount += product.Price * count;
    }

    /// <summary>
    /// 调整收货信息
    /// </summary>
    /// <param name="orderReceiver"></param>
    public void ChangeReceiver(OrderReceiver orderReceiver)
    {
        Checker.NotNull(orderReceiver, nameof(orderReceiver));
        this.Receiver = orderReceiver;
    }

    /// <summary>
    /// 标记订单创建状态
    /// </summary>
    /// <param name="isCreatedResult"></param>
    /// <param name="changesReason"></param>
    public void MarkCreatedStatus(bool isCreatedResult, string changesReason)
    {
        var status = isCreatedResult ? OrderStatusEnum.WaitPay : OrderStatusEnum.Canceling;

        this.ChangeStatus(status, changesReason);
    }

    /// <summary>
    /// 标记订单删除状态
    /// </summary>
    /// <param name="changesReason"></param>
    public void MarkDeletedStatus(string changesReason)
    {
        this.ChangeStatus(OrderStatusEnum.Deleted, changesReason);
    }

    /// <summary>
    /// 调整订单状态
    /// </summary>
    /// <param name="newStatus"></param>
    /// <param name="changesReason"></param>
    internal void ChangeStatus(OrderStatusEnum newStatus, string changesReason)
    {
        if (newStatus == OrderStatusEnum.Canceling)
        {
            if (this.Status.Code == OrderStatusEnum.WaitPay)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.Cancelled)
        {
            if (this.Status.Code == OrderStatusEnum.Canceling)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.Deleted)
        {
            if (this.Status.Code == OrderStatusEnum.Cancelled)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.Paying)
        {
            if (this.Status.Code == OrderStatusEnum.WaitPay)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.WaitSend)
        {
            if (this.Status.Code == OrderStatusEnum.Paying)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.WaitConfirm)
        {
            if (this.Status.Code == OrderStatusEnum.WaitSend)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.WaitRate)
        {
            if (this.Status.Code == OrderStatusEnum.WaitConfirm)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }

        if (newStatus == OrderStatusEnum.Finished)
        {
            if (this.Status.Code == OrderStatusEnum.WaitRate)
                this.Status = new OrderStatus(newStatus, changesReason);
            else
                throw new ArgumentException(nameof(newStatus));
        }
    }
}