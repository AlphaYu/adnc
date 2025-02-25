namespace Adnc.Demo.Usr.Api.Controllers;

/// <summary>
/// 组织机构管理
/// </summary>
[Route($"{RouteConsts.UsrRoot}/organizations")]
[ApiController]
public class OrganizationController : AdncControllerBase
{
    private readonly IOrganizationAppService _organizationService;

    public OrganizationController(IOrganizationAppService organizationService)
       => _organizationService = organizationService;

    /// <summary>
    /// 删除组织机构
    /// </summary>
    /// <param name="id">组织机构ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [AdncAuthorize(PermissionConsts.Dept.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] long id)
        => Result(await _organizationService.DeleteAsync(id));

    /// <summary>
    /// 新增组织机构
    /// </summary>
    /// <param name="input">组织机构</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Dept.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] OrganizationCreationDto input)
        => CreatedResult(await _organizationService.CreateAsync(input));

    /// <summary>
    /// 修改组织机构
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">组织机构</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Dept.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] OrganizationUpdationDto input)
        => Result(await _organizationService.UpdateAsync(id, input));

    /// <summary>
    /// 获取组织机构列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [AdncAuthorize(PermissionConsts.Dept.GetList, AdncAuthorizeAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OrganizationTreeDto>>> GetListAsync()
        => await _organizationService.GetTreeListAsync();
}