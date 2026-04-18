using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages users.
/// </summary>
[Route($"{RouteConsts.AdminRoot}/users")]
[ApiController]
public class UserController(IUserService userService) : AdncControllerBase
{
    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="input">The user to create.</param>
    /// <returns>The ID of the created user.</returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.User.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] UserCreationDto input)
        => CreatedResult(await userService.CreateAsync(input));

    /// <summary>
    /// Updates a user.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="input">The user changes.</param>
    /// <returns>A result indicating whether the user was updated.</returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.User.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] UserUpdationDto input)
        => Result(await userService.UpdateAsync(id, input));

    /// <summary>
    /// Deletes one or more users.
    /// </summary>
    /// <param name="ids">The comma-separated user IDs.</param>
    /// <returns>A result indicating whether the users were deleted.</returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.User.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        return Result(await userService.DeleteAsync(idArr));
    }

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="password">The new password.</param>
    /// <returns>A result indicating whether the password was reset.</returns>
    [HttpPatch("{id}/password")]
    [AdncAuthorize(PermissionConsts.User.ResetPassword)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ResetPasswordAsync([FromRoute] long id, string password)
        => Result(await userService.ResetPasswordAsync(id, password));

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The requested user.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AdncAuthorize([PermissionConsts.User.Get, PermissionConsts.User.Update])]
    public async Task<ActionResult<UserDto>> GetAsync([FromRoute] long id)
    {
        var user = await userService.GetAsync(id);
        return user is null ? NotFound() : user;
    }

    /// <summary>
    /// Gets a paged list of users.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of users.</returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.User.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<UserDto>>> GetPagedAsync([FromQuery] UserSearchPagedDto input)
        => await userService.GetPagedAsync(input);
}
