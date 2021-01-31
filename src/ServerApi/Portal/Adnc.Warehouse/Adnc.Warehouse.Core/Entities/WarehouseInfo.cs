using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class WarehouseInfo : AggregateRoot<long>
    {
        public long? ProductId { protected set; get; }
        public int Qty { protected set; get; }
        public int FreezedQty { protected set; get; }
        public string Shelf { protected get; set; }

        protected WarehouseInfo()
        {
        }

        internal WarehouseInfo(long productId,string shelf)
        {
            this.ProductId = productId;
            this.Shelf = shelf;
            this.Qty = 0;
            this.FreezedQty = 0;
        }

        internal void Freeze(int needFreezedQty)
        {
            if (this.Qty < needFreezedQty)
                throw new ArgumentException("Qty<needFreezedQty");

            this.FreezedQty += needFreezedQty;
            this.Qty -= needFreezedQty;
        }

        internal void Unfreeze(int needUnfreezeQty)
        {
            if (this.FreezedQty < needUnfreezeQty)
                throw new ArgumentException("FreezedQty<needUnfreezeQty");

            this.FreezedQty -= needUnfreezeQty;
            this.Qty += needUnfreezeQty;
        }

        internal void Outbound(int qty)
        {
            if (this.FreezedQty < qty)
                throw new ArgumentException("FreezedQty<qty");

            this.FreezedQty -= qty;
        }

        internal void Inbound(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("qty <= 0");
            this.Qty += qty;
        }
    }
}
