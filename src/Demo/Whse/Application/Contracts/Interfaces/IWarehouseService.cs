using Adnc.Demo.Remote.Event;
using Adnc.Demo.Whse.Application.Contracts.Dtos.Warehouse;

namespace Adnc.Demo.Whse.Application.Contracts.Interfaces;

/// <summary>
/// Warehouse management
/// </summary>
public interface IWarehouseService : IAppService
{
    /// <summary>
    /// Create a warehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "Create warehouse")]
    Task<WarehouseDto> CreateAsync(WarehouseCreationDto input);

    /// <summary>
    /// Allocate a warehouse shelf to a product
    /// </summary>
    /// <param name="warehouseId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [UnitOfWork()]
    [OperateLog(LogName = "Assign shelf")]
    Task<WarehouseDto> AllocateShelfToProductAsync(long warehouseId, WarehouseAllocateToProductDto input);

    /// <summary>
    /// Reserve product inventory
    /// </summary>
    /// <param name="eventDto"></param>
    ///  <param name="tracker"></param>
    /// <returns></returns>
    [UnitOfWork]
    [OperateLog(LogName = "Reserve inventory")]
    Task BlockQtyAsync(OrderCreatedEvent eventDto, IMessageTracker tracker);

    /// <summary>
    /// Get a paginated list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto input);
}
