using Adnc.Core.Shared.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Orders.Domain.Entities
{
    public class OrderStatus : ValueObject
    {
        public OrderStatusEnum StatusCode { get;}

        public string ChangeStatusReason { get;}

        private OrderStatus()
        {
        }

        public OrderStatus(OrderStatusEnum statusCode, string reason = null)
        {
            this.StatusCode = statusCode;
            this.ChangeStatusReason = reason ?? reason.Trim();
        }
    }

    public enum OrderStatusEnum
    {
        Creating = 1000
        ,
        WaitPay = 1008
        ,
        Paying = 1016
        ,
        WaitSend = 1040
        ,
        WaitConfirm = 1048
        ,
        WaitRate = 1056
        ,
        Finished = 1064
        ,
        Canceling = 1023
        ,
        Cancelled = 1024
        ,
        Deleted = 1032

    }
}