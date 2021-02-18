using Adnc.Core.Shared.Domain.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Orders.Domain.Entities
{
    /// <summary>
    /// 订单条目
    /// </summary>
    public class OrderItem : Entity
    {
        public long OrderId { get; private set; }

        public OrderItemProduct Product { get; private set; }

        public int Count { get; private set; }

        public decimal Amount { get; }

        private OrderItem() { }

        public OrderItem(long orderId, OrderItemProduct product, int count)
        {
            this.OrderId = Checker.GTZero(orderId, nameof(orderId));
            this.Product = Checker.NotNull(product, nameof(product));
            this.Count = Checker.GTZero(count, nameof(count));
            this.Id = product.Id;
        }

        public void ChangeCount(int count)
        {
            Checker.GTZero(count, nameof(count));
            this.Count += count;
        }
    }
}
