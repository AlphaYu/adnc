namespace Adnc.Maint.WebApi.Controllers;

/// <summary>
/// 字典管理
/// </summary>
[Route("maint/dicts")]
[ApiController]
public class DictController : AdncControllerBase
{
    private readonly IDictAppService _dictAppService;

    public DictController(IDictAppService dictAppService)
        => _dictAppService = dictAppService;

    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"><see cref="DictCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    [Permission(PermissionConsts.Dict.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] DictCreationDto input)
        => CreatedResult(await _dictAppService.CreateAsync(input));

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input"><see cref="DictUpdationDto"/></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Permission(PermissionConsts.Dict.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictUpdationDto input)
        => Result(await _dictAppService.UpdateAsync(id, input));

    /// <summary>
    /// 删除字典
    /// </summary>
    /// <param name="id">字典ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Permission(PermissionConsts.Dict.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        => Result(await _dictAppService.DeleteAsync(id));

    /// <summary>
    /// 获取字典列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Permission(PermissionConsts.Dict.GetList)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DictDto>>> GetListAsync([FromQuery] DictSearchDto search)
        => await _dictAppService.GetListAsync(search);

    /// <summary>
    /// 获取单个字典数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Permission(PermissionConsts.Dict.GetList, PermissionAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DictDto>> GetAsync([FromRoute] long id)
    {
        var cfg = await _dictAppService.GetAsync(id);
        if (cfg != null)
            return cfg;

        return NoContent();
    }
}