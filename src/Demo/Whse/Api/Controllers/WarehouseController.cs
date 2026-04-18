using Adnc.Demo.Whse.Application.Contracts.Dtos.Warehouse;

namespace Adnc.Demo.Whse.WebApi.Controllers;

/// <summary>
/// Warehouse shelf management
/// </summary>
[Route("whse/warehouses")]
[ApiController]
public class WarehouseController(IWarehouseService warehouseSrv) : AdncControllerBase
{
    /// <summary>
    /// Create a warehouse shelf
    /// </summary>
    /// <param name="input"><see cref="WarehouseCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<WarehouseDto>> CreateAsync([FromBody] WarehouseCreationDto input) => await warehouseSrv.CreateAsync(input);

    /// <summary>
    /// Assign the shelf to a product
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}/product")]
    public async Task<ActionResult<WarehouseDto>> AllocateShelfToProductAsync([FromRoute] long id, [FromBody] WarehouseAllocateToProductDto input) => await warehouseSrv.AllocateShelfToProductAsync(id, input);

    /// <summary>
    /// Get a paginated list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageModelDto<WarehouseDto>> GetPagedAsync([FromQuery] WarehouseSearchDto input) => await warehouseSrv.GetPagedAsync(input);
}
