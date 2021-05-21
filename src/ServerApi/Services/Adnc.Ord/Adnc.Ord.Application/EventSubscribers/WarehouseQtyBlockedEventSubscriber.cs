using Adnc.Infra.EventBus;
using Adnc.Ord.Application.Contracts.Dtos;
using Adnc.Ord.Application.Contracts.Services;
using DotNetCore.CAP;
using System.Threading.Tasks;

namespace Adnc.Ord.Application.EventSubscribers
{
    /// <summary>
    /// 库存锁定事件订阅者
    /// </summary>
    public class WarehouseQtyBlockedEventSubscriber : ICapSubscribe
    {
        private readonly IOrderAppService _orderAppSrv;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WarehouseQtyBlockedEventSubscriber(IOrderAppService orderAppSrv)
        {
            _orderAppSrv = orderAppSrv;
        }

        /// <summary>
        /// 事件处理程序
        /// </summary>
        /// <param name="warehouseQtyBlockedEvent"></param>
        /// <returns></returns>
        [CapSubscribe("WarehouseQtyBlockedEvent")]
        public async Task Process(BaseEvent<EventData> warehouseQtyBlockedEvent)
        {
            var data = warehouseQtyBlockedEvent.Data;
            await _orderAppSrv.MarkCreatedStatusAsync(data.OrderId, new OrderMarkCreatedStatusDto { IsSuccess = data.IsSuccess, Remark = data.Remark });
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