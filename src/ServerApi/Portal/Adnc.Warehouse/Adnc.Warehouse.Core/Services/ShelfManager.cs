using System;
using System.Threading.Tasks;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Warehouse.Core.Entities;
using Adnc.Warehouse.Core.EventBus;
using Adnc.Warehouse.Core.EventBus.Etos;
using DotNetCore.CAP;

namespace Adnc.Warehouse.Core.Services
{
    public class ShelfManager : ICoreService
    {
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Shelf> _shelfRepo;
        private readonly ICapPublisher _capBus;
        public ShelfManager(IEfRepository<Product> productRepo
            , IEfRepository<Shelf> shelfRepo
            , ICapPublisher capBus)
        {
            _productRepo = productRepo;
            _shelfRepo = shelfRepo;
            _capBus = capBus;
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
            var shelf = await _shelfRepo.FetchAsync(x => x, x => x.Position.Code == positionCode);
            if (shelf != null)
                throw new ArgumentException("warehouseInfo");

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
            var existShelf = await _shelfRepo.FetchAsync(x=>x,x => x.ProductId == product.Id);

            //一个商品只能分配一个货架，但可以调整货架。
            if (existShelf != null && existShelf.Id != shelf.Id)
                throw new ArgumentException("AssignedWarehouseId");

            shelf.SetProductId(product.Id);

            //发布领域事件，Product会订阅该事件，调整商品对应的货架号。
            await _capBus.PublishAsync(EventBusConsts.ShelfToProductAllocated
            , new ShelfToProductAllocatedEto
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
