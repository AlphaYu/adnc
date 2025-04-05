using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Whse.Application.Services.Implements;

/// <summary>
/// 仓储管理
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="warehouseManager"></param>
/// <param name="mapper"></param>
/// <param name="warehouseRepo"></param>
/// <param name="productRepo"></param>
public class WarehouseService(WarehouseManager warehouseManager, IMapper mapper, IEfBasicRepository<Warehouse> warehouseRepo, IEfBasicRepository<Product> productRepo)
    : AbstractAppService, IWarehouseService
{
    /// <summary>
    /// 创建仓储
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
    {
        input.TrimStringFields();
        var warehouse = await warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

        await warehouseRepo.InsertAsync(warehouse);

        return mapper.Map<WarehouseDto>(warehouse);
    }

    /// <summary>
    /// 分配仓储给商品
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

        return mapper.Map<WarehouseDto>(warehouse);
    }

    /// <summary>
    /// 分页列表
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
    /// 锁定库存
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

        //库存都符合锁定条件才能批量更新数据库
        if (result)
        {
            await warehouseRepo.UpdateRangeAsync(warehouses);
            await tracker.MarkAsProcessedAsync(eventDto);
        }
    }
}
