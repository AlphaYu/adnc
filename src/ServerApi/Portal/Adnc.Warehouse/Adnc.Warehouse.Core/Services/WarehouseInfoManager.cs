using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Warehouse.Core.Entities;
using Adnc.Warehouse.Core.EventBus;
using Adnc.Warehouse.Core.EventBus.Etos;

namespace Adnc.Warehouse.Core.Services
{
    public class WarehouseInfoManager
    {
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Shlef> _shelfRepo;
        private readonly Publisher _publisher;
        public WarehouseInfoManager(IEfRepository<Product> productRepo
            , IEfRepository<Shlef> shelfRepo
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
        public async Task<Shlef> CreateAsync(string code)
        {
            var shelf = await _shelfRepo.FetchAsync(x => x, x => x.Code == code);
            if (shelf != null)
                throw new ArgumentException("warehouseInfo");

            return new Shlef(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , code
            );
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AllocateShelfToProductAsync(Shlef shelf,Product product)
        {
            if (product.AssignedWarehouseId.HasValue)
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
