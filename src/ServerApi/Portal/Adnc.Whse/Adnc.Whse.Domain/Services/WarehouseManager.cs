using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Exceptions;
using Adnc.Whse.Domain.Entities;
using Adnc.Infr.EventBus;

namespace Adnc.Whse.Domain.Services
{
    public class WarehouseManager : ICoreService
    {
        private readonly IEfRepository<Product> _productRepo;
        private readonly IEfRepository<Warehouse> _warehouseRepo;
        private readonly IEventPublisher _eventPublisher;
        public WarehouseManager(IEfRepository<Product> productRepo
            , IEfRepository<Warehouse> warehouseRepo
            , IEventPublisher eventPublisher)
        {
            _productRepo = productRepo;
            _warehouseRepo = warehouseRepo;
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
        public async Task<Warehouse> CreateAsync(string positionCode, string positionDescription)
        {
            var shelf = await _warehouseRepo.FetchAsync(x => x, x => x.Position.Code == positionCode, noTracking: false);
            if (shelf != null)
                throw new AdncArgumentException("warehouseInfo");

            return new Warehouse(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , new WarehousePosition(positionCode, positionDescription)
            );
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AllocateShelfToProductAsync(Warehouse shelf, Product product)
        {
            Checker.NotNull(shelf, nameof(shelf));
            Checker.NotNull(product, nameof(product));

            var existShelf = await _warehouseRepo.FetchAsync(x => x, x => x.ProductId == product.Id, noTracking: false);

            //一个商品只能分配一个货架，但可以调整货架。
            if (existShelf != null && existShelf.Id != shelf.Id)
                throw new AdncArgumentException("AssignedWarehouseId", nameof(shelf));

            shelf.SetProductId(product.Id);

        }

        public async Task FreezeInventorys(long orderId, List<Warehouse> shelfs, Dictionary<long, int> products)
        {
            bool isSuccess = false;

            if (shelfs.Count == products.Count)
            {
                try
                {
                    //这里需要捕获业务逻辑的异常
                    foreach (var shelf in shelfs)
                    {
                        var qty = products[shelf.ProductId.Value];
                        shelf.BlockQty(qty);
                    }
                }
                catch (Exception ex)
                {

                }

                //成功冻结所有库存
                isSuccess = true;
            }

            //这里不需要捕获系统异常
            await _warehouseRepo.UpdateAsync(null);

            //发布冻结库存事件
            //await _eventPublisher.PublishAsync(""
            //,
            //new OrderInventoryFreezedEventEto
            //{
            //    Id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
            //    ,
            //    OrderId = orderId
            //    ,
            //    IsSuccess = isSuccess
            //    ,
            //    EventSource = nameof(ShelfManager.FreezeInventorys)
            //});
        }
    }
}
