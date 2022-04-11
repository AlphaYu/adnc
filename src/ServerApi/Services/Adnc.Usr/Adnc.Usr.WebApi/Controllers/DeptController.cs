namespace Adnc.Usr.WebApi.Controllers;

/// <summary>
/// 部门
/// </summary>
[Route("usr/depts")]
[ApiController]
public class DeptController : AdncControllerBase
{
    private readonly IDeptAppService _deptService;

    public DeptController(IDeptAppService deptService)
       => _deptService = deptService;

    /// <summary>
    /// 删除部门
    /// </summary>
    /// <param name="id">部门ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Permission(PermissionConsts.Dept.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] long id)
        => Result(await _deptService.DeleteAsync(id));

    /// <summary>
    /// 新增部门
    /// </summary>
    /// <param name="input">部门</param>
    /// <returns></returns>
    [HttpPost]
    [Permission(PermissionConsts.Dept.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<long>> CreateAsync([FromBody] DeptCreationDto input)
        => CreatedResult(await _deptService.CreateAsync(input));

    /// <summary>
    /// 修改部门
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">部门</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Permission(PermissionConsts.Dept.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DeptUpdationDto input)
        => Result(await _deptService.UpdateAsync(id, input));

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Permission(PermissionConsts.Dept.GetList, PermissionAttribute.JwtWithBasicSchemes)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DeptTreeDto>>> GetListAsync()
        => await _deptService.GetTreeListAsync();
}