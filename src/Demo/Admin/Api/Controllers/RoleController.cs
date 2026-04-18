using Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages roles.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/roles")]
[ApiController]
public class RoleController(IRoleService roleService) : AdncControllerBase
{
    /// <summary>
    /// Creates a role.
    /// </summary>
    /// <param name="input">The role to create.</param>
    /// <returns>The ID of the created role.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Role.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] RoleCreationDto input)
        => CreatedResult(await roleService.CreateAsync(input));

    /// <summary>
    /// Updates a role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="input">The role changes.</param>
    /// <returns>A result indicating whether the role was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.Role.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] RoleUpdationDto input)
        => Result(await roleService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more roles.
    /// </summary>
    /// <param name="ids">The comma-separated role IDs.</param>
    /// <returns>A result indicating whether the roles were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.Role.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(",").Select(long.Parse).ToArray();
        return Result(await roleService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Gets a paged list of roles.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of roles.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Role.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<RoleDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => await roleService.GetPagedAsync(input);

    /// <summary>
    /// Gets a role by ID.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The requested role.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AdncAuthorize([PermissionConsts.Role.Get, PermissionConsts.Role.Update])]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetAsync([FromRoute] long id)
    {
        var role = await roleService.GetAsync(id);
        return role is null ? NotFound() : role;
    }

    /// <summary>
    /// Saves the menu permissions assigned to a role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <param name="permissions">The menu permission IDs assigned to the role.</param>
    /// <returns>A result indicating whether the role permissions were saved.</returns>
    [HttpPatch("{id}/permissons")]
    [AdncAuthorize(PermissionConsts.Role.SetPermissons)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SetPermissonsAsync([FromRoute] long id, [FromBody] long[] permissions)
        => Result(await roleService.SetPermissonsAsync(new RoleSetPermissonsDto() { RoleId = id, Permissions = permissions }));

    /// <summary>
    /// Gets the menu IDs assigned to a role.
    /// </summary>
    /// <param name="id">The role ID.</param>
    /// <returns>The menu IDs assigned to the role.</returns>
    [HttpGet("{id}/menuids")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<long[]>> GetMenuIdsAsync([FromRoute] long id)
        => await roleService.GetMenuIdsAsync(id);

    /// <summary>
    /// Gets role options.
    /// </summary>
    /// <returns>The available role options.</returns>
    [HttpGet("options")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<OptionTreeDto>>> GetOptionsAsync()
        => await roleService.GetOptionsAsync();
}
