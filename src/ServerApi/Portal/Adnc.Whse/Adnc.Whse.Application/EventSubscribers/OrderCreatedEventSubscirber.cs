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
        private readonly IEfRepository<Shelf> _shelfReop;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shelfReop"><see cref="Shelf"/></param>
        public OrderCreatedEventSubscirber(IEfRepository<Shelf> shelfReop)
        {
            _shelfReop = shelfReop;
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
            var shelfs = await _shelfReop.Where(x => products.Keys.Contains(x.ProductId.Value), noTracking: false).ToListAsync();

            foreach (var produdct in eto.Data.Products)
            {
                var shelf = await _shelfReop.FetchAsync(x => x, x => x.ProductId == produdct.ProductId);
                shelf.FreezeInventory(produdct.Qty);
                await _shelfReop.UpdateAsync(shelf);
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
