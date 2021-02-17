using System;
using System.Linq;
using System.Collections.Generic;
using Adnc.Infr.Common.Exceptions;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Orders.Domain.Entities
{
    public class Order : AggregateRoot
    {

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


        public Order(long id, long customerId, ICollection<OrderItem> items, OrderDeliveryInfomation deliveryInfomation = null, string remark = null)
        {
            this.Id = Checker.GTZero(id, nameof(id));
            this.CustomerId = Checker.GTZero(customerId, nameof(customerId));
            this.Items = Checker.NotNullOrEmpty(items, nameof(items));
            this.DeliveryInfomaton = deliveryInfomation;
            this.Remark = remark;
            this.Amount = items.Select(x => x.Count * x.Product.Price).Sum();
        }

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
        }
    }
}
