namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 字典管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/dicts")]
[ApiController]
public class DictController(IDictService dictService)
    : AdncControllerBase
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"><see cref="DictCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Dict.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] DictCreationDto input)
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
        var idArr = ids.Split(',').Select(x => long.Parse(x)).ToArray();
        return Result(await dictService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取字典列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Dict.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<DictDto>>> GetPagedAsync([FromQuery] DictSearchPagedDto search)
        => await dictService.GetPagedAsync(search);

    /// <summary>
    /// 获取单个字典
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DictDto>> GetAsync([FromRoute] long id)
    {
        var dict = await dictService.GetAsync(id);
        return dict is null ? NotFound() : dict;
    }

    /// <summary>
    /// 获取字典数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    //[AdncAuthorize(PermissionConsts.Dict.GetList, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    public async Task<ActionResult<List<DictOption>>> GetOptionsAsync()
    => await dictService.GetOptionsAsync();

}