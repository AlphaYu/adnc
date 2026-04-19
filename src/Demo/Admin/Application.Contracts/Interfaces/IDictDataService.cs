using Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines dictionary data management services.
/// </summary>
public interface IDictDataService : IAppService
{
    /// <summary>
    /// Creates a dictionary data entry.
    /// </summary>
    /// <param name="input">The dictionary data entry to create.</param>
    /// <returns>The ID of the created dictionary data entry.</returns>
    [OperateLog(LogName = "Create dictionary data")]
    [CachingEvict(CacheKey = CacheConsts.DictOptionsListKey)]
    Task<ServiceResult<IdDto>> CreateAsync(DictDataCreationDto input);

    /// <summary>
    /// Updates a dictionary data entry.
    /// </summary>
    /// <param name="id">The dictionary data entry ID.</param>
    /// <param name="input">The dictionary data entry changes.</param>
    /// <returns>A result indicating whether the dictionary data entry was updated.</returns>
    [OperateLog(LogName = "Update dictionary data")]
    [CachingEvict(CacheKey = CacheConsts.DictOptionsListKey)]
    Task<ServiceResult> UpdateAsync(long id, DictDataUpdationDto input);

    /// <summary>
    /// Deletes one or more dictionary data entries.
    /// </summary>
    /// <param name="ids">The dictionary data entry IDs to delete.</param>
    /// <returns>A result indicating whether the dictionary data entries were deleted.</returns>
    [OperateLog(LogName = "Delete dictionary data")]
    [CachingEvict(CacheKey = CacheConsts.DictOptionsListKey)]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// Gets a dictionary data entry by ID.
    /// </summary>
    /// <param name="id">The dictionary data entry ID.</param>
    /// <returns>The requested dictionary data entry, or <c>null</c> if it does not exist.</returns>
    Task<DictDataDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of dictionary data entries.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of dictionary data entries.</returns>
    Task<PageModelDto<DictDataDto>> GetPagedAsync(DictDataSearchPagedDto input);
}
