namespace Adnc.Demo.Admin.Application.Contracts.Services;

/// <summary>
/// 字典管理
/// </summary>
public interface IDictService : IAppService
{
    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "新增字典")]
    Task<ServiceResult<IdDto>> CreateAsync(DictCreationDto input);

    /// <summary>
    /// 修改字典
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "修改字典")]
    [CachingEvict(CacheKey = CachingConsts.DictOptionsListKey)]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync(long id, DictUpdationDto input);

    /// <summary>
    /// 删除字典
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [OperateLog(LogName = "删除字典")]
    [CachingEvict(CacheKey = CachingConsts.DictOptionsListKey)]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// 获取单个字典
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DictDto?> GetAsync(long id);

    /// <summary>
    /// 获取字典列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageModelDto<DictDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 获取字典选项
    /// </summary>
    /// <returns></returns>
    Task<List<DictOptionDto>> GetOptionsAsync(string codes);
}
