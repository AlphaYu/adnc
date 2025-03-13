namespace Adnc.Demo.Usr.Api.Controllers;

/// <summary>
/// 组织机构管理
/// </summary>
[Route($"{RouteConsts.UsrRoot}/organizations")]
[ApiController]
public class OrganizationController(IOrganizationAppService organizationService) : AdncControllerBase
{
    /// <summary>
    /// 新增组织机构
    /// </summary>
    /// <param name="input">组织机构</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Org.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] OrganizationCreationDto input)
        => CreatedResult(await organizationService.CreateAsync(input));

    /// <summary>
    /// 修改组织机构
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">组织机构</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Org.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] OrganizationUpdationDto input)
        => Result(await organizationService.UpdateAsync(id, input));

    /// <summary>
    /// 删除组织机构
    /// </summary>
    /// <param name="ids">组织机构ID</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Org.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] string ids)
    {
        var idArr = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
        return Result(await organizationService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 获取组织机构信息
    /// </summary>
    /// <param name="id">组织机构ID</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    // [AdncAuthorize(PermissionConsts.Org.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<OrganizationDto?>> GetAsync([FromRoute] long id)
        => await organizationService.GetAsync(id);

    /// <summary>
    /// 获取组织机构列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.Org.GetList, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrganizationTreeDto>>> GetTreeListAsync(string? keywords = null, bool? status = null)
        => await organizationService.GetTreeListAsync(keywords, status);

    /// <summary>
    /// 获取取组织机构选项
    /// </summary>
    /// <returns></returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetOrgOptionsAsync()
        =>await  organizationService.GetOrgOptionsAsync(true);
}