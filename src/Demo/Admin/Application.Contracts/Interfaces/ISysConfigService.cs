using Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines system configuration management services.
/// </summary>
public interface ISysConfigService : IAppService
{
    /// <summary>
    /// Creates a system configuration.
    /// </summary>
    /// <param name="input">The configuration to create.</param>
    /// <returns>The ID of the created configuration.</returns>
    [OperateLog(LogName = "Create configuration")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult<IdDto>> CreateAsync(SysConfigCreationDto input);

    /// <summary>
    /// Updates a system configuration.
    /// </summary>
    /// <param name="id">The configuration ID.</param>
    /// <param name="input">The configuration changes.</param>
    /// <returns>A result indicating whether the configuration was updated.</returns>
    [OperateLog(LogName = "Update configuration")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult> UpdateAsync([CachingParam] long id, SysConfigUpdationDto input);

    /// <summary>
    /// Deletes one or more system configurations.
    /// </summary>
    /// <param name="ids">The configuration IDs to delete.</param>
    /// <returns>A result indicating whether the configurations were deleted.</returns>
    [OperateLog(LogName = "Delete configuration")]
    [CachingEvict(CacheKey = CachingConsts.SysConfigListCacheKey)]
    Task<ServiceResult> DeleteAsync([CachingParam] long[] ids);

    /// <summary>
    /// Gets a system configuration by ID.
    /// </summary>
    /// <param name="id">The configuration ID.</param>
    /// <returns>The requested configuration, or <c>null</c> if it does not exist.</returns>
    Task<SysConfigDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of system configurations.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of system configurations.</returns>
    Task<PageModelDto<SysConfigDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// Gets system configurations by key list.
    /// </summary>
    /// <param name="keys">The configuration keys, or <c>all</c>.</param>
    /// <returns>The matching configurations.</returns>
    Task<List<SysConfigSimpleDto>> GetListAsync(string keys);
}
