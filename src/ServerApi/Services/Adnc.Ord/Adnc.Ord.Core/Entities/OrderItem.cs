using Adnc.Core.Shared.Entities;
using Adnc.Infr.Common.Exceptions;

namespace Adnc.Ord.Core.Entities
{
    /// <summary>
    /// 订单条目
    /// </summary>
    public class OrderItem : Entity
    {
        public long OrderId { get; private set; }

        public OrderItemProduct Product { get; private set; }

        public int Count { get; private set; }

        //public decimal Amount { get; private set; }

        private OrderItem() { }

        internal OrderItem(long orderId, OrderItemProduct product, int count)
        {
            this.Id = product.Id;
            this.OrderId = Checker.GTZero(orderId, nameof(orderId));
            this.Product = Checker.NotNull(product, nameof(product));
            this.Count = Checker.GTZero(count, nameof(count));
            //this.Amount = product.Price * count;
        }

        internal void ChangeCount(int count)
        {
            Checker.GTZero(count, nameof(count));
            this.Count += count;
            //this.Amount += this.Product.Price * count;
        }
    }
}
