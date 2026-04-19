using Adnc.Demo.Remote.Event;
using Adnc.Demo.Whse.Application.Contracts.Dtos.Warehouse;
using Adnc.Infra.EventBus.Tracker;

namespace Adnc.Demo.Whse.Application.Services;

/// <summary>
/// Warehouse management
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="warehouseManager"></param>
/// <param name="warehouseRepo"></param>
/// <param name="productRepo"></param>
public class WarehouseService(WarehouseManager warehouseManager, IEfBasicRepository<Warehouse> warehouseRepo, IEfBasicRepository<Product> productRepo)
    : AbstractAppService, IWarehouseService
{
    /// <summary>
    /// Create a warehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
    {
        input.TrimStringFields();
        var warehouse = await warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

        await warehouseRepo.InsertAsync(warehouse);

        return Mapper.Map<WarehouseDto>(warehouse);
    }

    /// <summary>
    /// Allocate a warehouse shelf to a product
    /// </summary>
    /// <param name="warehouseId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WarehouseDto> AllocateShelfToProductAsync(long warehouseId, WarehouseAllocateToProductDto input)
    {
        input.TrimStringFields();
        var warehouse = await warehouseRepo.GetRequiredAsync(warehouseId);

        await warehouseManager.AllocateShelfToProductAsync(warehouse, input.ProductId);

        await warehouseRepo.UpdateAsync(warehouse);

        return Mapper.Map<WarehouseDto>(warehouse);
    }

    /// <summary>
    /// Get a paginated list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto input)
    {
        input.TrimStringFields();
        var total = await warehouseRepo.CountAsync(x => true);

        if (total == 0)
        {
            return new PageModelDto<WarehouseDto>(input);
        }

        var products = productRepo.Where(x => true);
        var warehouses = warehouseRepo.Where(x => true);
        var data = await (from s in warehouses
                          join p in products
                          on s.ProductId equals p.Id into sp
                          from x in sp.DefaultIfEmpty()
                          select new WarehouseDto()
                          {
                              Id = s.Id,
                              FreezedQty = s.BlockedQty,
                              PositionCode = s.Position.Code,
                              PositionDescription = s.Position.Description,
                              ProductId = s.ProductId,
                              ProductName = x.Name,
                              ProductSku = x.Sku,
                              Qty = s.Qty
                          })
                       .Skip(input.SkipRows())
                       .Take(input.PageSize)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();

        return new PageModelDto<WarehouseDto>(input, data, total);
    }

    /// <summary>
    /// Reserve inventory
    /// </summary>
    /// <param name="eventDto"></param>
    /// <param name="tracker"></param>
    /// <returns></returns>
    public async Task BlockQtyAsync(OrderCreatedEvent eventDto, IMessageTracker tracker)
    {
        eventDto.TrimStringFields();
        var blockQtyProductsInfo = eventDto.Products.ToDictionary(x => x.ProductId, x => x.Qty);
        var warehouses = await warehouseRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.ProductId), noTracking: false).ToListAsync();
        // var products = await productRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.Id)).ToListAsync();

        var result = await warehouseManager.BlockQtyAsync(eventDto.OrderId, blockQtyProductsInfo, warehouses);

        // Only update the database in batch when all inventory entries satisfy the reservation conditions.
        if (result)
        {
            await warehouseRepo.UpdateRangeAsync(warehouses);
            await tracker.MarkAsProcessedAsync(eventDto);
        }
    }
}
