﻿using System.Threading.Tasks;
using System.Collections.Generic;
using DotNetCore.CAP;
using Adnc.Infr.EventBus;
using Adnc.Whse.Application.Services;
using Adnc.Whse.Application.Dtos;

namespace Adnc.Whse.Application.EventSubscribers
{
    /// <summary>
    /// 订单创建事件订阅者
    /// </summary>
    public class OrderCreatedEventSubscirber : ICapSubscribe
    {
        private readonly IWarehouseAppService _warehouseSrv;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="warehouseSrv"><see cref="IWarehouseAppService"/></param>
        public OrderCreatedEventSubscirber(IWarehouseAppService warehouseSrv)
        {
            _warehouseSrv = warehouseSrv;
        }

        /// <summary>
        /// 事件处理程序
        /// </summary>
        /// <param name="orderCreatedEvent"></param>
        /// <returns></returns>
        [CapSubscribe("OrderCreatedEvent")]
        public async Task Process(BaseEvent<EventData> orderCreatedEvent)
        {
            await _warehouseSrv.BlockQtyAsync(new WarehouseBlockQtyDto { OrderId=orderCreatedEvent.Data.OrderId,Products = orderCreatedEvent.Data.Products });
        }

        /// <summary>
        /// 订单创建事件数据
        /// </summary>
        public class EventData
        {
            public long OrderId { get; set; }

            public ICollection<(long ProductId, int Qty)> Products { get; set; }
        }
    }
}
