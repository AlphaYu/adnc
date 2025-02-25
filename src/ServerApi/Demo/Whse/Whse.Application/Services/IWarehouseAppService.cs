namespace Adnc.Demo.Whse.Application.Services;

/// <summary>
/// 仓储管理
/// </summary>
public interface IWarehouseAppService : IAppService
{
    /// <summary>
    /// 创建仓储
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "创建仓储")]
    Task<WarehouseDto> CreateAsync(WarehouseCreationDto input);

    /// <summary>
    /// 分配仓储给商品
    /// </summary>
    /// <param name="warehouseId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork()]
    [OperateLog(LogName = "分配货架")]
    Task<WarehouseDto> AllocateShelfToProductAsync(long warehouseId, WarehouseAllocateToProductDto input);

    /// <summary>
    /// 锁定商品库存
    /// </summary>
    /// <param name="eventDto"></param>
    ///  <param name="tracker"></param>
    /// <returns></returns>
    [UnitOfWork]
    [OperateLog(LogName = "锁定库存")]
    Task BlockQtyAsync(OrderCreatedEvent eventDto, IMessageTracker tracker);

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search);
}