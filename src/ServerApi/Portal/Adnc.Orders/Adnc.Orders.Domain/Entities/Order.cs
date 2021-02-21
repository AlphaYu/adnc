using System;
using System.Linq;
using System.Collections.Generic;
using Adnc.Infr.Common.Exceptions;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Orders.Domain.Entities
{
    public class Order : AggregateRoot
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
        public string Remark { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// 收货信息
        /// </summary>
        public OrderDeliveryInfomation DeliveryInfomaton { get; private set; }

        /// <summary>
        /// 订单子项
        /// </summary>
        public virtual ICollection<OrderItem> Items { get; private set; }

        private Order() { }


        public Order(long id, long customerId, OrderDeliveryInfomation deliveryInfomation = null, string remark = null)
        {
            this.Id = Checker.GTZero(id, nameof(id));
            this.CustomerId = Checker.GTZero(customerId, nameof(customerId));
            this.Items = new List<OrderItem>();
            this.DeliveryInfomaton = deliveryInfomation;
            this.Status = new OrderStatus(OrderStatusEnum.Creating);
            this.Remark = remark;
            this.Amount = 0;
        }

        /// <summary>
        /// 添加订单产品
        /// </summary>
        /// <param name="product"></param>
        /// <param name="count"></param>
        public void AddProduct(OrderItemProduct product, int count)
        {
            Checker.NotNull(product, nameof(product));
            Checker.GTZero(count, nameof(count));

            var existProduct = this.Items.FirstOrDefault(x => x.Product.Id == product.Id);
            if (existProduct == null)
            {
                this.Items.Add(new OrderItem(this.Id, product, count));
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
        /// <param name="deliveryInfomation"></param>
        public void ChangeDeliveryInfomation(OrderDeliveryInfomation deliveryInfomation)
        {
            Checker.NotNull(deliveryInfomation, nameof(deliveryInfomation));
            this.DeliveryInfomaton = deliveryInfomation;
        }

        /// <summary>
        /// 调整订单状态
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatus(OrderStatusEnum newStatus)
        {
            if (newStatus == OrderStatusEnum.Canceling)
            {
                if (this.Status.StatusCode == OrderStatusEnum.WaitPay)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.Cancelled)
            {
                if (this.Status.StatusCode == OrderStatusEnum.Canceling)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.Deleted)
            {
                if (this.Status.StatusCode == OrderStatusEnum.Cancelled)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.Paying)
            {
                if (this.Status.StatusCode == OrderStatusEnum.WaitPay)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.WaitSend)
            {
                if (this.Status.StatusCode == OrderStatusEnum.Paying)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.WaitConfirm)
            {
                if (this.Status.StatusCode == OrderStatusEnum.WaitSend)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }

            if (newStatus == OrderStatusEnum.WaitRate)
            {
                if (this.Status.StatusCode == OrderStatusEnum.WaitConfirm)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }


            if (newStatus == OrderStatusEnum.Finished)
            {
                if (this.Status.StatusCode == OrderStatusEnum.WaitRate)
                    this.Status = new OrderStatus(newStatus);
                else
                    throw new Exception();
            }
        }
    }
}
