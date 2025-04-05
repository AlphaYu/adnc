namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 机构服务
/// </summary>
public interface IOrganizationService : IAppService
{
    /// <summary>
    /// 新增机构
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增机构")]
    [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey })]
    Task<ServiceResult<IdDto>> CreateAsync(OrganizationCreationDto input);

    /// <summary>
    /// 修改机构
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改机构")]
    [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey })]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync(long id, OrganizationUpdationDto input);

    /// <summary>
    /// 删除机构
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除机构")]
    [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey })]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// 获取机构信息
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    Task<OrganizationDto?> GetAsync(long Id);

    /// <summary>
    /// 机构树结构
    /// </summary>
    /// <returns></returns>
    //[CachingAble(CacheKey = CachingConsts.DetpTreeListCacheKey, Expiration = CachingConsts.OneYear)]
    Task<List<OrganizationTreeDto>> GetTreeListAsync(string? name = null, bool? status = null);

    Task<List<OptionTreeDto>> GetOrgOptionsAsync(bool? status = null);
}
