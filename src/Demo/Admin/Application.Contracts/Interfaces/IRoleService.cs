using Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

namespace Adnc.Demo.Admin.Application.Contracts.Interfaces;

/// <summary>
/// Defines role services.
/// </summary>
public interface IRoleService : IAppService
{
    /// <summary>
    /// Creates a role.
    /// </summary>
    /// <param name="input">The role to create.</param>
    /// <returns>The ID of the created role.</returns>
    [OperateLog(LogName = "Create role")]
    Task<ServiceResult<IdDto>> CreateAsync(RoleCreationDto input);

    /// <summary>
    /// Updates a role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="input">The role changes.</param>
    /// <returns>A result indicating whether the role was updated.</returns>
    [OperateLog(LogName = "Update role")]
    Task<ServiceResult> UpdateAsync(long id, RoleUpdationDto input);

    /// <summary>
    /// Deletes one or more roles.
    /// </summary>
    /// <param name="ids">The role IDs to delete.</param>
    /// <returns>A result indicating whether the roles were deleted.</returns>
    [OperateLog(LogName = "Delete role")]
    [UnitOfWork]
    Task<ServiceResult> DeleteAsync(long[] ids);

    /// <summary>
    /// Gets a role by ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The requested role, or <c>null</c> if it does not exist.</returns>
    Task<RoleDto?> GetAsync(long id);

    /// <summary>
    /// Gets a paged list of roles.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of roles.</returns>
    Task<PageModelDto<RoleDto>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// Sets permissions for a role.
    /// </summary>
    /// <param name="input">The role permission assignment.</param>
    /// <returns>A result indicating whether the permissions were saved.</returns>
    [OperateLog(LogName = "Set role permissions")]
    [UnitOfWork]
    Task<ServiceResult> SetPermissonsAsync(RoleSetPermissonsDto input);

    /// <summary>
    /// Gets the menu IDs assigned to a role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The menu IDs assigned to the role.</returns>
    Task<long[]> GetMenuIdsAsync(long id);

    /// <summary>
    /// Gets role options.
    /// </summary>
    /// <param name="status">The optional role status filter.</param>
    /// <returns>The available role options.</returns>
    Task<List<OptionTreeDto>> GetOptionsAsync(bool? status = null);
}
