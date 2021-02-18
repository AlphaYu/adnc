using System.Threading.Tasks;
using Adnc.Core.Shared.IRepositories;
using Adnc.Warehouse.Domain.Entities;
using Adnc.Warehouse.Domain.Events;
using DotNetCore.CAP;

namespace Adnc.Warehouse.Application.EventSubscribers
{
    /// <summary>
    /// 分配货架事件订阅者
    /// </summary>
    public class ShelfToProductAllocatedEventSubscirber : ICapSubscribe
    {
        private readonly IEfRepository<Product> _productReop;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productReop"><see cref="Product"/></param>
        public ShelfToProductAllocatedEventSubscirber(IEfRepository<Product> productReop)
        {
            _productReop = productReop;
        }

        /// <summary>
        /// 事件处理逻辑
        /// </summary>
        /// <param name="eto"></param>
        /// <returns></returns>
        [CapSubscribe(EventConsts.ShelfToProductAllocatedEvent)]
        public async Task Process(ShelfToProductAllocatedEventEto eto)
        {
            var produdct = await _productReop.FindAsync(eto.ProductId, noTracking: false);
            if (produdct != null)
            {
                produdct.SetShelf(eto.ShelfId);
                await _productReop.UpdateAsync(produdct);
            }
        }
    }
}
