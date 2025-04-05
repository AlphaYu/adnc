namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 配置管理
/// </summary>
public interface ISysConfigService : IAppService
{
    /// <summary>
    /// 新增配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增配置")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult<IdDto>> CreateAsync(SysConfigCreationDto input);

    /// <summary>
    /// 修改配置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改配置")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult> UpdateAsync([CachingParam] long id, SysConfigUpdationDto input);

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除配置")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult> DeleteAsync([CachingParam] long[] ids);

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<SysConfigDto?> GetAsync(long id);

    /// <summary>
    /// 配置列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<SysConfigDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 依据Key获取配置
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    Task<List<SysConfigSimpleDto>> GetListAsync(string keys);
}
