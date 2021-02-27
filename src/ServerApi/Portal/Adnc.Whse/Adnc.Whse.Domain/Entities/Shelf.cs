using System;
using Adnc.Infr.Common.Exceptions;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Whse.Domain.Entities
{
    /// <summary>
    /// 货架
    /// </summary>
    public class Shelf : AggregateRootWithBasicAuditInfo
    {
        public long? ProductId { get; private set; }

        public int Qty { get; private set; }

        public int FreezedQty { get; private set; }

        public ShelfPosition Position { get; private set; }

        private Shelf()
        {
        }

        internal Shelf(long id, ShelfPosition position)
        {
            this.Id = id;
            this.Qty = 0;
            this.FreezedQty = 0;
            this.Position = Checker.NotNull(position, nameof(position));
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="needFreezedQty"></param>
        public void FreezeInventory(int needFreezedQty)
        {
            if (this.Qty < needFreezedQty)
                throw new AdncArgumentException("Qty<needFreezedQty", nameof(needFreezedQty));
            if (!this.ProductId.HasValue)
                throw new AdncArgumentNullException("ProductId", nameof(ProductId));
            this.FreezedQty += needFreezedQty;
            this.Qty -= needFreezedQty;
        }

        /// <summary>
        /// 解冻库存
        /// </summary>
        /// <param name="needUnfreezeQty"></param>
        internal void UnfreezeInventory(int needUnfreezeQty)
        {
            if (this.FreezedQty < needUnfreezeQty)
                throw new AdncArgumentException("FreezedQty<needUnfreezeQty", nameof(needUnfreezeQty));
            if (!this.ProductId.HasValue)
                throw new ArgumentNullException("ProductId", nameof(ProductId));

            this.FreezedQty -= needUnfreezeQty;
            this.Qty += needUnfreezeQty;
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="qty"></param>
        internal void Outbound(int qty)
        {
            if (this.FreezedQty < qty)
                throw new AdncArgumentException("FreezedQty<qty", nameof(qty));
            if (!this.ProductId.HasValue)
                throw new AdncArgumentException("ProductId", nameof(ProductId));
            this.FreezedQty -= qty;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="qty"></param>
        internal void Inbound(int qty)
        {
            if (qty <= 0)
                throw new AdncArgumentException("qty <= 0", nameof(qty));
            if (!this.ProductId.HasValue)
                throw new AdncArgumentException("ProductId", nameof(ProductId));
            this.Qty += qty;
        }


        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="productId"></param>
        internal void SetProductId(long productId)
        {
            //if (this.ProductId.HasValue && this.ProductId == productId)
            //    throw new ArgumentException("ProductId");

            if (productId == 0)
                throw new ArgumentException("ProductId");

            this.ProductId = productId;
        }
    }
}
