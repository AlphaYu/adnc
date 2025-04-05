namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 角色服务
/// </summary>
public interface IRoleService : IAppService
{
    /// <summary>
    /// 新增角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增角色")]
    Task<ServiceResult<IdDto>> CreateAsync(RoleCreationDto input);

    /// <summary>
    /// 修改角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改角色")]
    Task<ServiceResult> UpdateAsync(long id, RoleUpdationDto input);

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除角色")]
    [UnitOfWork]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// 获取角色信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<RoleDto?> GetAsync(long id);

    /// <summary>
    /// 获取角色列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<RoleDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 设置角色权限
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "设置角色权限")]
    [UnitOfWork]
    Task<ServiceResult> SetPermissonsAsync(RoleSetPermissonsDto input);

    /// <summary>
    /// 获取用户拥有的角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<long[]> GetMenuIdsAsync(long id);

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    Task<List<OptionTreeDto>> GetOptionsAsync(bool? status = null);
}
