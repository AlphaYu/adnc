using Adnc.Demo.Whse.Domain.Aggregates;
using Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

namespace Adnc.Demo.Whse.Domain.Services;

public class WarehouseManager : IDomainService
{
    private readonly IEfBasicRepository<Warehouse> _warehouseRepo;

    public WarehouseManager(IEfBasicRepository<Warehouse> warehouseRepo) => _warehouseRepo = warehouseRepo;

    /// <summary>
    /// 创建货架
    /// </summary>
    /// <param name="positionCode">位置代码</param>
    /// <param name="positionDescription">位置描述</param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async Task<Warehouse> CreateAsync(string positionCode, string positionDescription)
    {
        var exists = await _warehouseRepo.AnyAsync(x => x.Position.Code == positionCode);
        if (exists)
            throw new BusinessException($"positionCode exists({positionCode})");

        return new Warehouse(
            IdGenerater.GetNextId()
            , new WarehousePosition(positionCode, positionDescription)
        );
    }

    /// <summary>
    /// 分配货架给商品
    /// </summary>
    /// <param name="warehouse"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task AllocateShelfToProductAsync(Warehouse warehouse, long productId)
    {
        Guard.Checker.NotNull(warehouse, nameof(warehouse));

        var existWarehouse = await _warehouseRepo.Where(x => x.ProductId == productId).SingleOrDefaultAsync();

        //一个商品只能分配一个货架，但可以调整货架。
        if (existWarehouse != null && existWarehouse.Id != warehouse.Id)
            throw new BusinessException($"exist warehouse ({productId})");

        warehouse.SetProductId(productId);
    }

    /// <summary>
    /// 锁定库存
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="blockQtyProductsInfo"></param>
    /// <param name="warehouses"></param>
    /// <returns></returns>
    public async Task<bool> BlockQtyAsync(long orderId, Dictionary<long, int> blockQtyProductsInfo, List<Warehouse> warehouses)
    {
        bool isSuccess = false;
        string remark = string.Empty;

        Guard.Checker.NotNullOrAny(blockQtyProductsInfo, nameof(blockQtyProductsInfo));
        Guard.Checker.NotNullOrAny(warehouses, nameof(warehouses));

        if (orderId <= 0)
            remark += $"{orderId}订单号错误";
        else if (blockQtyProductsInfo.Count == 0)
            remark += $"商品数量为空";
        else if (warehouses.Count == 0)
            remark += $"仓储数量为空";
        else if (warehouses.Count != blockQtyProductsInfo.Count)
            remark += remark + $"商品数量与库存数量不一致";
        else
        {
            try
            {
                //这里需要捕获业务逻辑的异常
                foreach (var productId in blockQtyProductsInfo.Keys)
                {
                    var needBlockQty = blockQtyProductsInfo[productId];
                    var warehouse = warehouses.FirstOrDefault(x => x.ProductId == productId);
                    warehouse.BlockQty(needBlockQty);
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
        var warehouseQtyBlockedEvent = new WarehouseQtyBlockedEvent
        {
            Id = IdGenerater.GetNextId(),
            EventSource = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty,
            OrderId = orderId,
            IsSuccess = isSuccess,
            Remark = remark 
        };
        await warehouses[0].EventPublisher.Value.PublishAsync(warehouseQtyBlockedEvent);

        return isSuccess;
    }
}