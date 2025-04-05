namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 字典数据管理
/// </summary>
public interface IDictDataService : IAppService
{
    /// <summary>
    /// 新增字典数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增字典数据")]
    [CachingEvict(CacheKey = CachingConsts.DictOptionsListKey)]
    Task<ServiceResult<IdDto>> CreateAsync(DictDataCreationDto input);

    /// <summary>
    /// 修改字典数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改字典数据")]
    [CachingEvict(CacheKey = CachingConsts.DictOptionsListKey)]
    Task<ServiceResult> UpdateAsync(long id, DictDataUpdationDto input);

    /// <summary>
    /// 删除字典数据
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除字典数据")]
    [CachingEvict(CacheKey = CachingConsts.DictOptionsListKey)]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// 获取单个字典数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DictDataDto?> GetAsync(long id);

    /// <summary>
    /// 获取字典数据列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<DictDataDto>> GetPagedAsync(DictDataSearchPagedDto input);
}
