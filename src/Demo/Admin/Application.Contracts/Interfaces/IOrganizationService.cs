using Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines organization management services.
/// </summary>
public interface IOrganizationService : IAppService
{
    /// <summary>
    /// Creates an organization.
    /// </summary>
    /// <param name="input">The organization to create.</param>
    /// <returns>The ID of the created organization.</returns>
    [OperateLog(LogName = "Create organization")]
    [CachingEvict(CacheKeys = new[] { CacheConsts.DetpListCacheKey })]
    Task<ServiceResult<IdDto>> CreateAsync(OrganizationCreationDto input);

    /// <summary>
    /// Updates an organization.
    /// </summary>
    /// <param name="id">The organization ID.</param>
    /// <param name="input">The organization changes.</param>
    /// <returns>A result indicating whether the organization was updated.</returns>
    [OperateLog(LogName = "Update organization")]
    [CachingEvict(CacheKeys = new[] { CacheConsts.DetpListCacheKey })]
    [UnitOfWork]
    Task<ServiceResult> UpdateAsync(long id, OrganizationUpdationDto input);

    /// <summary>
    /// Deletes one or more organizations.
    /// </summary>
    /// <param name="ids">The organization IDs to delete.</param>
    /// <returns>A result indicating whether the organizations were deleted.</returns>
    [OperateLog(LogName = "Delete organization")]
    [CachingEvict(CacheKeys = new[] { CacheConsts.DetpListCacheKey })]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// Gets an organization by ID.
    /// </summary>
    /// <param name="id">The organization ID.</param>
    /// <returns>The requested organization, or <c>null</c> if it does not exist.</returns>
    Task<OrganizationDto?> GetAsync(long id);

    /// <summary>
    /// Gets the organization tree.
    /// </summary>
    /// <param name="name">The optional organization name filter.</param>
    /// <param name="status">The optional organization status filter.</param>
    /// <returns>The organization tree.</returns>
    Task<List<OrganizationTreeDto>> GetTreeListAsync(string? name = null, bool? status = null);

    /// <summary>
    /// Gets organization options.
    /// </summary>
    /// <param name="status">The optional organization status filter.</param>
    /// <returns>The organization option tree.</returns>
    Task<List<OptionTreeDto>> GetOrgOptionsAsync(bool? status = null);
}
