using Adnc.Core.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities
{
    public class ProductStatus : ValueObject
    {
        public ProductStatusEnum StatusCode { get; private set; }
        public string ChangeStatusReason { get; private set; }

        private ProductStatus()
        {
        }

        public ProductStatus(ProductStatusEnum statusCode,string reason)
        {
            this.StatusCode = statusCode;
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
