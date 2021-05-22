using Adnc.Core.Shared.Entities;

namespace Adnc.Ord.Core.Entities
{
    public class OrderStatus : ValueObject
    {
        public OrderStatusEnum Code { get; }

        public string ChangesReason { get; }

        private OrderStatus()
        {
        }

        public OrderStatus(OrderStatusEnum statusCode, string reason = null)
        {
            this.Code = statusCode;
            this.ChangesReason = reason != null ? reason.Trim() : string.Empty;
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