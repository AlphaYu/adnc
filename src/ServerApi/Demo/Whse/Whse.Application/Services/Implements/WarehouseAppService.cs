namespace Adnc.Demo.Whse.Application.Services.Implements;

/// <summary>
/// 仓储管理
/// </summary>
public class WarehouseAppService : AbstractAppService, IWarehouseAppService
{
    private readonly IMapper _mapper;
    private readonly IEfBasicRepository<Warehouse> _warehouseRepo;
    private readonly IEfBasicRepository<Product> _productRepo;
    private readonly WarehouseManager _warehouseManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="warehouseManager"></param>
    /// <param name="mapper"></param>
    /// <param name="warehouseRepo"></param>
    /// <param name="productRepo"></param>
    public WarehouseAppService(WarehouseManager warehouseManager
        , IMapper mapper
        , IEfBasicRepository<Warehouse> warehouseRepo
        , IEfBasicRepository<Product> productRepo)
    {
        _warehouseManager = warehouseManager;
        _warehouseRepo = warehouseRepo;
        _productRepo = productRepo;
        _mapper = mapper;
    }

    /// <summary>
    /// 创建仓储
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WarehouseDto> CreateAsync(WarehouseCreationDto input)
    {
        input.TrimStringFields();
        var warehouse = await _warehouseManager.CreateAsync(input.PositionCode, input.PositionDescription);

        await _warehouseRepo.InsertAsync(warehouse);

        return _mapper.Map<WarehouseDto>(warehouse);
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
        var warehouse = await _warehouseRepo.GetAsync(warehouseId);

        await _warehouseManager.AllocateShelfToProductAsync(warehouse, input.ProductId);

        await _warehouseRepo.UpdateAsync(warehouse);

        return _mapper.Map<WarehouseDto>(warehouse);
    }

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public async Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search)
    {
        search.TrimStringFields();
        var total = await _warehouseRepo.CountAsync(x => true);

        if (total == 0)
            return new PageModelDto<WarehouseDto>(search);

        var products = _productRepo.Where(x => true);
        var warehouses = _warehouseRepo.Where(x => true);
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
                       .Skip(search.SkipRows())
                       .Take(search.PageSize)
                       .OrderByDescending(x => x.Id)
                       .ToListAsync();

        return new PageModelDto<WarehouseDto>(search, data, total);
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
        var warehouses = await _warehouseRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.ProductId.Value), noTracking: false).ToListAsync();
        // var products = await _productRepo.Where(x => blockQtyProductsInfo.Keys.Contains(x.Id)).ToListAsync();

        var result = await _warehouseManager.BlockQtyAsync(eventDto.OrderId, blockQtyProductsInfo, warehouses);

        //库存都符合锁定条件才能批量更新数据库
        if (result)
        {
            await _warehouseRepo.UpdateRangeAsync(warehouses);
            await tracker?.MarkAsProcessedAsync(eventDto);
        }
    }
}