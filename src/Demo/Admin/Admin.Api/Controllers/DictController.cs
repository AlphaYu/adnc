namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 字典管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/dicts")]
[ApiController]
public class DictController(IDictService dictService) : AdncControllerBase
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"><see cref="DictCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Dict.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] DictCreationDto input)
        => CreatedResult(await dictService.CreateAsync(input));

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input"><see cref="DictUpdationDto"/></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Dict.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictUpdationDto input)
        => Result(await dictService.UpdateAsync(id, input));

    /// <summary>
    /// 删除字典
    /// </summary>
    /// <param name="ids">字典ID</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Dict.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',').Select(long.Parse).ToArray();
        return Result(await dictService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取字典分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{DictDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Dict.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<DictDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => await dictService.GetPagedAsync(input);

    /// <summary>
    /// 获取单个字典
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [AdncAuthorize([PermissionConsts.Dict.Get, PermissionConsts.Dict.Update])]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DictDto>> GetAsync([FromRoute] long id)
    {
        var dict = await dictService.GetAsync(id);
        return dict is null ? NotFound() : dict;
    }

    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <param name="codes">字典编码</param>
    /// <returns><see cref="List{DictOption}"/></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DictOptionDto>>> GetOptionsAsync([FromQuery] string codes)
        => await dictService.GetOptionsAsync(codes);
}
