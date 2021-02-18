using System.Threading.Tasks;
using System.Linq;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Warehouse.Domain.Entities;
using Adnc.Warehouse.Domain.Events;
using DotNetCore.CAP;

namespace Adnc.Warehouse.Application.EventSubscribers
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
        public async Task<OrderInventoryFreezedEventEto> Process(OrderCreatedEventEto eto)
        {
            bool isSuccess = false;

            var orderId = eto.OrderId;
            var products = eto.Products.ToDictionary(x => x.ProductId, x => x.Qty);
            var shelfs = await _shelfReop.Where(x => products.Keys.Contains(x.ProductId.Value), noTracking: false).ToListAsync();



            //foreach (var produdct in eto.Products)
            //{
            //    var shelf = await _shelfReop.FetchAsync(x => x, x => x.ProductId == produdct.ProductId);
            //    shelf.FreezeInventory(produdct.Qty);
            //    await _shelfReop.UpdateAsync(shelf);
            //}

            isSuccess = true;

            return new OrderInventoryFreezedEventEto
            {
                Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                ,
                OrderId = eto.OrderId
                ,
                IsSuccess = isSuccess
            };
        }
    }
}
