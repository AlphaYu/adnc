using Adnc.Core.Shared.Entities;
using Adnc.Infra.Common.Exceptions;
using System;

namespace Adnc.Whse.Core.Entities
{
    /// <summary>
    /// 货架
    /// </summary>
    public class Warehouse : AggregateRootWithBasicAuditInfo
    {
        public long? ProductId { get; private set; }

        public int Qty { get; private set; }

        public int BlockedQty { get; private set; }

        public WarehousePosition Position { get; private set; }

        private Warehouse()
        {
        }

        internal Warehouse(long id, WarehousePosition position)
        {
            this.Id = id;
            this.Qty = 0;
            this.BlockedQty = 0;
            this.Position = Checker.NotNull(position, nameof(position));
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="needBlockedQty"></param>
        internal void BlockQty(int needBlockedQty)
        {
            if (this.Qty < needBlockedQty)
                throw new AdncArgumentException("Qty<needFreezedQty", nameof(needBlockedQty));
            if (!this.ProductId.HasValue)
                throw new AdncArgumentNullException("ProductId", nameof(ProductId));
            this.BlockedQty += needBlockedQty;
            this.Qty -= needBlockedQty;
        }

        /// <summary>
        /// 移除被冻结的库存
        /// </summary>
        /// <param name="needRemoveQty"></param>
        internal void RemoveBlockedQty(int needRemoveQty)
        {
            if (this.BlockedQty < needRemoveQty)
                throw new AdncArgumentException("FreezedQty<needUnfreezeQty", nameof(needRemoveQty));
            if (!this.ProductId.HasValue)
                throw new ArgumentNullException("ProductId", nameof(ProductId));

            this.BlockedQty -= needRemoveQty;
            this.Qty += needRemoveQty;
        }

        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="qty"></param>
        internal void Deliver(int qty)
        {
            if (this.BlockedQty < qty)
                throw new AdncArgumentException("FreezedQty<qty", nameof(qty));
            if (!this.ProductId.HasValue)
                throw new AdncArgumentException("ProductId", nameof(ProductId));
            this.BlockedQty -= qty;
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="qty"></param>
        internal void Entry(int qty)
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