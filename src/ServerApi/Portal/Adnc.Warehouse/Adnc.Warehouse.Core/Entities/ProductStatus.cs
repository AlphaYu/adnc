using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class ProductStatus : ValueObject
    {
        public ProductStatusEnum Status { get; private set; }
        public string ChangeStatusReason { get; private set; }

        private ProductStatus()
        {
        }

        public ProductStatus(ProductStatusEnum status,string reason)
        {
            this.Status = status;
            this.ChangeStatusReason = reason;
        }
    }

    public enum ProductStatusEnum
    {
        UnKnow = 1000
        ,
        SaleOff = 1003
        ,
        SaleOn = 1006
    }
}
