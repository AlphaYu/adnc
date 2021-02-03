using System;
using System.Threading.Tasks;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Warehouse.Core.Entities;
using Adnc.Warehouse.Core.EventBus;
using Adnc.Warehouse.Core.EventBus.Etos;

namespace Adnc.Warehouse.Core.Services
{
    public class ShelfManager
    {
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Shelf> _shelfRepo;
        private readonly Publisher _publisher;
        public ShelfManager(IEfRepository<Product> productRepo
            , IEfRepository<Shelf> shelfRepo
            , Publisher publisher)
        {
            _productRepo = productRepo;
            _shelfRepo = shelfRepo;
            _publisher = publisher;
        }

        /// <summary>
        /// 创建货架
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="name"></param>
        /// <param name="unit"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        public async Task<Shelf> CreateAsync(string positionCode,string positionDescription)
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
        public async Task AllocateShelfToProductAsync(Shelf shelf,Product product)
        {
            if (product.ShlefId.HasValue)
                throw new ArgumentException("AssignedWarehouseId");

            var exists = await _shelfRepo.ExistAsync(x => x.ProductId == product.Id);
            if(exists)
                throw new ArgumentException("AssignedWarehouseId");

            shelf.SetProductId(product.Id);

            //发布领域事件
            await _publisher.PublishAsync(EventBusConsts.ShelfToProductAllocated, new ShelfToProductAllocatedEto
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
