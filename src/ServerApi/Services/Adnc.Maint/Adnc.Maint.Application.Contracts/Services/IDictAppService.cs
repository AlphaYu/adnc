namespace Adnc.Maint.Application.Contracts.Services;

/// <summary>
/// 字典管理
/// </summary>
public interface IDictAppService : IAppService
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增字典")]
    [CachingEvict(CacheKey = CachingConsts.DictListCacheKey)]
    Task<AppSrvResult<long>> CreateAsync(DictCreationDto input);

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改字典")]
    [CachingEvict(CacheKey = CachingConsts.DictListCacheKey)]
    [UnitOfWork]
    Task<AppSrvResult> UpdateAsync(long id, DictUpdationDto input);

    /// <summary>
    /// 删除字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除字典")]
    [CachingEvict(CacheKey = CachingConsts.DictListCacheKey)]
    Task<AppSrvResult> DeleteAsync(long id);

    /// <summary>
    /// 字典列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<List<DictDto>> GetListAsync(DictSearchDto search);

    /// <summary>
    /// 获取字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DictDto> GetAsync(long id);
}