using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using DotNetCore.CAP;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.EventBus;
using Adnc.Whse.Domain.Entities;

namespace Adnc.Whse.Application.EventSubscribers
{
    /// <summary>
    /// 订单创建事件订阅者
    /// </summary>
    public class OrderCreatedEventSubscirber : ICapSubscribe
    {
        private readonly IEfBasicRepository<Warehouse> _warehouseRepo;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="warehouseRepo"><see cref="Warehouse"/></param>
        public OrderCreatedEventSubscirber(IEfBasicRepository<Warehouse> warehouseRepo)
        {
            _warehouseRepo = warehouseRepo;
        }

        /// <summary>
        /// 冻结库存
        /// </summary>
        /// <param name="eto"></param>
        /// <returns></returns>
        [CapSubscribe("OrderCreatedEvent")]
        public async Task Process(BaseEvent<EventData> eto)
        {
            var orderId = eto.Data.OrderId;
            var products = eto.Data.Products.ToDictionary(x => x.ProductId, x => x.Qty);
            var warehouses = await _warehouseRepo.Where(x => products.Keys.Contains(x.ProductId.Value), noTracking: false).ToListAsync();

            foreach (var produdct in eto.Data.Products)
            {
                var warehouse = warehouses.Where(x => x.ProductId == produdct.ProductId).SingleOrDefault();
                warehouse.BlockQty(produdct.Qty);
                await _warehouseRepo.UpdateAsync(warehouse);
            }
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
