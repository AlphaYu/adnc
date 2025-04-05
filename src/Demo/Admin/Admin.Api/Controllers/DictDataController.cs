namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 字典数据管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/dictdatas")]
[ApiController]
public class DictDataController(IDictDataService dictDataService) : AdncControllerBase
{
    /// <summary>
    /// 新增字典数据
    /// </summary>
    /// <param name="input"><see cref="DictDataCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.DictData.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] DictDataCreationDto input)
        => CreatedResult(await dictDataService.CreateAsync(input));

    /// <summary>
    /// 修改字典数据
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input"><see cref="DictDataUpdationDto"/></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.DictData.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictDataUpdationDto input)
        => Result(await dictDataService.UpdateAsync(id, input));

    /// <summary>
    /// 删除字典数据
    /// </summary>
    /// <param name="ids">字典ID</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.DictData.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await dictDataService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取字典数据列表
    /// </summary>
    /// <returns><see cref="PageModelDto{DictDataDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.DictData.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<DictDataDto>>> GetPagedAsync([FromQuery] DictDataSearchPagedDto input)
        => await dictDataService.GetPagedAsync(input);

    /// <summary>
    /// 获取单个字典数据
    /// </summary>
    /// <returns><see cref="DictDataDto"/></returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.DictData.Get, PermissionConsts.DictData.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DictDataDto>> GetAsync([FromRoute] long id)
    {
        var dictData = await dictDataService.GetAsync(id);
        return dictData is null ? NotFound() : dictData;
    }
}
