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
        private readonly IEfRepository<WarehouseInfo> _warehouseInfoRepo;
        private readonly Publisher _publisher;
        public WarehouseInfoManager(IEfRepository<Product> productRepo
            , IEfRepository<WarehouseInfo> warehouseInfoRepo
            , Publisher publisher)
        {
            _productRepo = productRepo;
            _warehouseInfoRepo = warehouseInfoRepo;
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
        public async Task<WarehouseInfo> CreateAsync(string shelf)
        {
            var warehouseInfo = await _warehouseInfoRepo.FetchAsync(x => x, x => x.Shelf == shelf);
            if (warehouseInfo != null)
                throw new ArgumentException("warehouseInfo");

            return new WarehouseInfo(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , shelf
            );
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="warehouseInfo"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AllocateShelfToProductAsync(WarehouseInfo warehouseInfo,Product product)
        {
            if (product.AssignedWarehouseId.HasValue)
                throw new ArgumentException("AssignedWarehouseId");

            var exists = await _warehouseInfoRepo.ExistAsync(x => x.ProductId == product.Id);
            if(exists)
                throw new ArgumentException("AssignedWarehouseId");

            warehouseInfo.SetProductId(product.Id);

            //发布领域事件
            await _publisher.PublishAsync(EventBusConsts.ShelfToProductAllocated, new ShelfToProductAllocatedEto
            {
                Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                ,
                ShelfId = warehouseInfo.Id
                ,
                ProductId = product.Id
                ,
                EventSource = nameof(this.AllocateShelfToProductAsync)
            });
        }
    }
}
