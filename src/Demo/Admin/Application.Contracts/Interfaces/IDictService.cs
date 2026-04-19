using Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines dictionary management services.
/// </summary>
public interface IDictService : IAppService
{
    /// <summary>
    /// Creates a dictionary.
    /// </summary>
    /// <param name="input">The dictionary to create.</param>
    /// <returns>The ID of the created dictionary.</returns>
    [OperateLog(LogName = "Create dictionary")]
    Task<ServiceResult<IdDto>> CreateAsync(DictCreationDto input);

    /// <summary>
    /// Updates a dictionary.
    /// </summary>
    /// <param name="id">The dictionary ID.</param>
    /// <param name="input">The dictionary changes.</param>
    /// <returns>A result indicating whether the dictionary was updated.</returns>
    [OperateLog(LogName = "Update dictionary")]
    [CachingEvict(CacheKey = CacheConsts.DictOptionsListKey)]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync(long id, DictUpdationDto input);

    /// <summary>
    /// Deletes one or more dictionaries.
    /// </summary>
    /// <param name="ids">The dictionary IDs to delete.</param>
    /// <returns>A result indicating whether the dictionaries were deleted.</returns>
    [OperateLog(LogName = "Delete dictionary")]
    [CachingEvict(CacheKey = CacheConsts.DictOptionsListKey)]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// Gets a dictionary by ID.
    /// </summary>
    /// <param name="id">The dictionary ID.</param>
    /// <returns>The requested dictionary, or <c>null</c> if it does not exist.</returns>
    Task<DictDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of dictionaries.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of dictionaries.</returns>
    Task<PageModelDto<DictDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// Gets dictionary options by code list.
    /// </summary>
    /// <param name="codes">The dictionary codes.</param>
    /// <returns>The matching dictionary options.</returns>
    Task<List<DictOptionDto>> GetOptionsAsync(string codes);
}
