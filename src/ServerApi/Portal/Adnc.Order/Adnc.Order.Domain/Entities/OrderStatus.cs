using Adnc.Core.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Orders.Domain.Entities
{
    public class OrderStatus : ValueObject
    {
        public OrderStatusEnum StatusCode { get; private set; }

        public string StatusName { get; private set; }

        public string ChangeStatusReason { get; private set; }

        private OrderStatus()
        {
        }

        public OrderStatus(OrderStatusEnum statusCode, string reason)
        {
            this.StatusCode = statusCode;
            this.ChangeStatusReason = reason ?? reason.Trim();
        }
    }

    public enum OrderStatusEnum
    {
        UnKnow = 1000
        ,
        SaleOff = 1003
        ,
        SaleOn = 1006
    }
}
