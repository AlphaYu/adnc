using System;
using System.Threading.Tasks;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Exceptions;
using Adnc.Warehouse.Domain.Entities;
using Adnc.Warehouse.Domain.Events;
using Adnc.Warehouse.Domain.Events.Etos;
using Adnc.Core.Shared.Events;

namespace Adnc.Warehouse.Domain.Services
{
    public class ShelfManager : ICoreService
    {
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Shelf> _shelfRepo;
        private readonly IEventPublisher _eventPublisher;
        public ShelfManager(IEfRepository<Product> productRepo
            , IEfRepository<Shelf> shelfRepo
            , IEventPublisher eventPublisher)
        {
            _productRepo = productRepo;
            _shelfRepo = shelfRepo;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 创建货架
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public async Task<Shelf> CreateAsync(string positionCode, string positionDescription)
        {
            var shelf = await _shelfRepo.FetchAsync(x => x, x => x.Position.Code == positionCode, noTracking: false);
            if (shelf != null)
                throw new AdncArgumentException("warehouseInfo");

            return new Shelf(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , new ShelfPosition(positionCode, positionDescription)
            );
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AllocateShelfToProductAsync(Shelf shelf, Product product)
        {
            var existShelf = await _shelfRepo.FetchAsync(x => x, x => x.ProductId == product.Id, noTracking: false);

            //一个商品只能分配一个货架，但可以调整货架。
            if (existShelf != null && existShelf.Id != shelf.Id)
                throw new AdncArgumentException("AssignedWarehouseId", nameof(shelf));

            shelf.SetProductId(product.Id);

            //发布领域事件，Product会订阅该事件，调整商品对应的货架号。
            await _eventPublisher.PublishAsync(EventConsts.ShelfToProductAllocatedEvent
            , new ShelfToProductAllocatedEventEto
            {
                Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                ,
                ShelfId = shelf.Id
                ,
                ProductId = product.Id
                ,
                EventSource = nameof(this.AllocateShelfToProductAsync)
            });
        }
    }
}
