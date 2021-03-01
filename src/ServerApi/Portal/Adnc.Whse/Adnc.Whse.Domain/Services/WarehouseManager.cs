using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Adnc.Core.Shared;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Common.Exceptions;
using Adnc.Whse.Domain.Entities;
using Adnc.Infr.EventBus;
using Adnc.Whse.Domain.Events;

namespace Adnc.Whse.Domain.Services
{
    public class WarehouseManager : ICoreService
    {
        private readonly IEfBasicRepository<Product> _productRepo;
        private readonly IEfBasicRepository<Warehouse> _warehouseRepo;
        private readonly IEventPublisher _eventPublisher;
        public WarehouseManager(IEfBasicRepository<Product> productRepo
            , IEfBasicRepository<Warehouse> warehouseRepo
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
            var exists = await _warehouseRepo.AnyAsync(x => x.Position.Code == positionCode);
            if (exists)
                throw new AdncArgumentException("warehouseInfo");

            return new Warehouse(
                IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId)
                , new WarehousePosition(positionCode, positionDescription)
            );
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <param name="warehouse"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task AllocateShelfToProductAsync(Warehouse warehouse, Product product)
        {
            Checker.NotNull(warehouse, nameof(warehouse));
            Checker.NotNull(product, nameof(product));

            var existWarehouse = await _warehouseRepo.Where(x => x.ProductId == product.Id).SingleOrDefaultAsync();

            //一个商品只能分配一个货架，但可以调整货架。
            if (existWarehouse != null && existWarehouse.Id != warehouse.Id)
                throw new AdncArgumentException("AssignedWarehouseId", nameof(warehouse));

            warehouse.SetProductId(product.Id);

        }

        /// <summary>
        /// 锁定库存
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="blockQtyProductsInfo"></param>
        /// <param name="warehouses"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task<bool> BlockQtyAsync(long orderId, Dictionary<long, int> blockQtyProductsInfo, List<Warehouse> warehouses, List<Product> products)
        {
            bool isSuccess = false;
            string remark = string.Empty;

            if (orderId <= 0)
                remark +=$"{orderId}订单号错误";
            else if (blockQtyProductsInfo?.Count == 0)
                remark += $"商品数量为空";
            else if (warehouses?.Count == 0)
                remark += $"仓储数量为空";
            else if (products?.Count == 0)
                remark += remark + $"产品数量为空";
            else if (warehouses.Count != blockQtyProductsInfo.Count)
                remark += remark + $"商品数量与库存数量不一致";
            else
            {
                try
                {
                    //这里需要捕获业务逻辑的异常
                    foreach (var productId in blockQtyProductsInfo.Keys)
                    {
                        var product = products.FirstOrDefault(x => x.Id == productId);

                        if (product == null)
                            remark += $"{productId}已经被删除;";
                        else if (product.Status.Code != ProductStatusEnum.SaleOn)
                            remark += $"{productId}已经下架;";
                        else
                        {
                            var needBlockQty = blockQtyProductsInfo[productId];
                            var warehouse = warehouses.FirstOrDefault(x => x.ProductId == productId);
                            warehouse.BlockQty(needBlockQty);
                        }
                    }
                }
                catch (Exception ex)
                {
                    remark += ex.Message;
                }
            }

            //成功冻结所有库存
            isSuccess = string.IsNullOrEmpty(remark);

            //发布冻结库存事件(不管是否冻结成功)
            var eventId = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
            var eventData = new WarehouseQtyBlockedEvent.EventData() { OrderId = orderId, IsSuccess = isSuccess, Remark = remark };
            var eventSource = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName;
            await _eventPublisher.PublishAsync(new WarehouseQtyBlockedEvent(eventId, eventData, eventSource));

            return isSuccess;
        }
    }
}
