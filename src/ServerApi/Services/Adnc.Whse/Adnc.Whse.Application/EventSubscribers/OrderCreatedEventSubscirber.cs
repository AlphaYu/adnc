using Adnc.Shared.Events;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Application.Contracts.Services;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Adnc.Whse.Application.EventSubscribers
{
    public sealed class CapEventSubscriber : ICapSubscribe
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CapEventSubscriber> _logger;

        public CapEventSubscriber(
            IServiceProvider services
            , ILogger<CapEventSubscriber> logger)
        {
            _services = services;
            _logger = logger;
        }

        #region across service event

        /// <summary>
        /// 订阅订单创建事件
        /// </summary>
        /// <param name="warehouseQtyBlockedEvent"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(OrderCreatedEvent))]
        public async Task ProcessWarehouseQtyBlockedEvent(OrderCreatedEvent eventObj)

        {
            var data = eventObj.Data;
            using var scope = _services.CreateScope();
            var appSrv = scope.ServiceProvider.GetRequiredService<IWarehouseAppService>();
            await appSrv.BlockQtyAsync(new WarehouseBlockQtyDto { OrderId = data.OrderId, Products = data.Products });
        }

        #endregion across service event
    }
}