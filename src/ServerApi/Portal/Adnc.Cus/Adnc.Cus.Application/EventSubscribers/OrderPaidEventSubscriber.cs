using System.Threading.Tasks;
using DotNetCore.CAP;
using Adnc.Infr.EventBus;
using Adnc.Cus.Application.Services;
using Adnc.Cus.Application.Dtos;

namespace Adnc.Cus.Application.EventSubscribers
{
    /// <summary>
    /// 订单付款事件订阅者，客户中心需要扣款
    /// </summary>
    public class OrderPaidEventSubscriber : ICapSubscribe
    {
        private readonly ICustomerAppService _customerAppSrv;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderPaidEventSubscriber(ICustomerAppService customerAppSrv)
        {
            _customerAppSrv = customerAppSrv;
        }

        /// <summary>
        /// 事件处理程序
        /// </summary>
        /// <param name="warehouseQtyBlockedEvent"></param>
        /// <returns></returns>
        [CapSubscribe("OrderPaidEvent")]
        public async Task Process(BaseEvent<EventData> warehouseQtyBlockedEvent)
        {
            var data = warehouseQtyBlockedEvent.Data;
            //await _orderAppSrv.MarkCreatedStatusAsync(data.OrderId, new OrderMarkCreatedStatusDto { IsSuccess = data.IsSuccess, Remark = data.Remark });
            return;
        }

        /// <summary>
        /// 事件数据
        /// </summary>
        public class EventData
        {
            public long OrderId { get; set; }

            public bool IsSuccess { get; set; }

            public string Remark { get; set; }
        }
    }
}
