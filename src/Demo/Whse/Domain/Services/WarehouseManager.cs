using Adnc.Demo.Remote.Event;
using Adnc.Demo.Whse.Domain.Aggregates.WarehouseAggregate;

namespace Adnc.Demo.Whse.Domain.Services;

public class WarehouseManager(IEfBasicRepository<Warehouse> warehouseRepo) : IDomainService
{
    /// <summary>
    /// Create a warehouse shelf
    /// </summary>
    /// <param name="positionCode">Position code</param>
    /// <param name="positionDescription">Position description</param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public async Task<Warehouse> CreateAsync(string positionCode, string positionDescription)
    {
        var exists = await warehouseRepo.AnyAsync(x => x.Position.Code == positionCode);
        if (exists)
        {
            throw new BusinessException($"positionCode exists({positionCode})");
        }

        return new Warehouse(
            IdGenerater.GetNextId()
            , new WarehousePosition(positionCode, positionDescription)
        );
    }

    /// <summary>
    /// Assign the shelf to a product
    /// </summary>
    /// <param name="warehouse"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task AllocateShelfToProductAsync(Warehouse warehouse, long productId)
    {
        Checker.Variable.NotNull(warehouse, nameof(warehouse));

        var existWarehouse = await warehouseRepo.Where(x => x.ProductId == productId).SingleOrDefaultAsync();

        // A product can only be assigned to one shelf, but the assigned shelf can be changed.
        if (existWarehouse != null && existWarehouse.Id != warehouse.Id)
        {
            throw new BusinessException($"exist warehouse ({productId})");
        }

        warehouse.SetProductId(productId);
    }

    /// <summary>
    /// Reserve inventory
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="blockQtyProductsInfo"></param>
    /// <param name="warehouses"></param>
    /// <returns></returns>
    public async Task<bool> BlockQtyAsync(long orderId, Dictionary<long, int> blockQtyProductsInfo, List<Warehouse> warehouses)
    {
        var isSuccess = false;
        var remark = string.Empty;

        Checker.Variable.NotNullOrAny(blockQtyProductsInfo, nameof(blockQtyProductsInfo));
        Checker.Variable.NotNullOrAny(warehouses, nameof(warehouses));

        if (orderId <= 0)
        {
            remark += $"{orderId} has an invalid order ID";
        }
        else if (blockQtyProductsInfo.Count == 0)
        {
            remark += "Product quantity is empty";
        }
        else if (warehouses.Count == 0)
        {
            remark += "Warehouse quantity is empty";
        }
        else if (warehouses.Count != blockQtyProductsInfo.Count)
        {
            remark += remark + "Product quantity does not match inventory quantity";
        }
        else
        {
            try
            {
                // Business logic exceptions need to be caught here.
                foreach (var productId in blockQtyProductsInfo.Keys)
                {
                    var needBlockQty = blockQtyProductsInfo[productId];
                    var warehouse = warehouses.First(x => x.ProductId == productId);
                    warehouse.BlockQty(needBlockQty);
                }
            }
            catch (Exception ex)
            {
                remark += ex.Message;
            }
        }

        // All inventory reservations succeeded.
        isSuccess = string.IsNullOrEmpty(remark);

        // Publish the inventory reservation event, regardless of success.
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
