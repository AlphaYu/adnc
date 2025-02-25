namespace Adnc.Demo.Maint.Application.Services;

/// <summary>
/// 配置管理
/// </summary>
public interface ICfgAppService : IAppService
{
    /// <summary>
    /// 新增配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增配置")]
    Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input);

    /// <summary>
    /// 修改配置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改配置")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.CfgSingleKeyPrefix)]
    Task<AppSrvResult> UpdateAsync([CachingParam] long id, CfgUpdationDto input);

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除配置")]
    [CachingEvict(CacheKeyPrefix = CachingConsts.CfgSingleKeyPrefix)]
    Task<AppSrvResult> DeleteAsync([CachingParam] long id);

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [CachingAble(CacheKeyPrefix = CachingConsts.CfgSingleKeyPrefix, Expiration = GeneralConsts.OneMonth)]
    Task<CfgDto> GetAsync([CachingParam] long id);

    /// <summary>
    /// 配置列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<PageModelDto<CfgDto>> GetPagedAsync(CfgSearchPagedDto search);
}