namespace Adnc.Demo.Whse.WebApi.Controllers;

/// <summary>
/// 货架管理
/// </summary>
[Route("whse/warehouses")]
[ApiController]
public class WarehouseController(IWarehouseService warehouseSrv) : AdncControllerBase
{
    /// <summary>
    /// 新建货架
    /// </summary>
    /// <param name="input"><see cref="WarehouseCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<WarehouseDto>> CreateAsync([FromBody] WarehouseCreationDto input) => await warehouseSrv.CreateAsync(input);

    /// <summary>
    /// 分配货架给商品
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}/product")]
    public async Task<ActionResult<WarehouseDto>> AllocateShelfToProductAsync([FromRoute] long id, [FromBody] WarehouseAllocateToProductDto input) => await warehouseSrv.AllocateShelfToProductAsync(id, input);

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PageModelDto<WarehouseDto>> GetPagedAsync([FromQuery] WarehouseSearchDto input) => await warehouseSrv.GetPagedAsync(input);
}
