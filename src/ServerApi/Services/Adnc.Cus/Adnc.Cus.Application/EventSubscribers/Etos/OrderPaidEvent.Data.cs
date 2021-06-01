namespace Adnc.Cus.Application.EventSubscribers.Etos
{
    public class OrderPaidEventData
    {
        public long OrderId { get; set; }

        public bool IsSuccess { get; set; }

        public string Remark { get; set; }
    }
}