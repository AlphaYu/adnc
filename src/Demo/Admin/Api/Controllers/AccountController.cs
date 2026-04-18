using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// Manages the current authenticated user session and profile.
/// </summary>
[Route($"{RouteConsts.AuthRoot}/session")]
[ApiController]
public class AccountController(IOptions<JWTOptions> jwtOptions, UserContext userContext, IUserService userService, ILogger<AccountController> logger)
    : AdncControllerBase
{
    /// <summary>
    /// Signs in a user and issues access and refresh tokens.
    /// </summary>
    /// <param name="input">The login credentials.</param>
    /// <returns>The issued access token and refresh token.</returns>
    [AllowAnonymous]
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UserTokenInfoDto>> LoginAsync([FromBody] UserLoginDto input)
    {
        var result = await userService.LoginAsync(input);
        if (result.IsSuccess)
        {
            var validatedInfo = result.Content;
            var accessToken = JwtTokenHelper.CreateAccessToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Account, validatedInfo.Id.ToString(), validatedInfo.Name, validatedInfo.GetRoleIdsString(), BearerDefaults.Manager);
            var refreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Id.ToString());
            var tokenInfo = new UserTokenInfoDto(accessToken.Token, accessToken.Expire, refreshToken.Token, refreshToken.Expire);
            return Created($"/auth/session", tokenInfo);
        }
        return Problem(result.ProblemDetails);
    }

    /// <summary>
    /// Signs out the current user.
    /// </summary>
    /// <returns>A result indicating whether the current session was removed.</returns>
    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> LogoutAsync()
        => Result(await userService.DeleteUserValidateInfoAsync(userContext.Id));

    /// <summary>
    /// Refreshes the access token by using a refresh token.
    /// </summary>
    /// <param name="input">The refresh token payload.</param>
    /// <returns>The refreshed access token and refresh token pair.</returns>
    [AllowAnonymous, HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserTokenInfoDto>> RefreshAccessTokenAsync([FromBody] UserRefreshTokenDto input)
    {
        var claimOfId = JwtTokenHelper.GetClaimFromRefeshToken(jwtOptions.Value, input.RefreshToken, JwtRegisteredClaimNames.NameId);
        if (claimOfId is not null)
        {
            var id = claimOfId.Value.ToLong();
            if (id is null)
            {
                return Forbid();
            }

            var validatedInfo = await userService.GetUserValidatedInfoAsync(id.Value);
            if (validatedInfo is null)
            {
                return Forbid();
            }

            var jti = JwtTokenHelper.GetClaimFromRefeshToken(jwtOptions.Value, input.RefreshToken, JwtRegisteredClaimNames.Jti);
            if (jti is null || jti.Value != validatedInfo.ValidationVersion)
            {
                return Forbid();
            }

            var accessToken = JwtTokenHelper.CreateAccessToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Account, validatedInfo.Id.ToString(), validatedInfo.Name, validatedInfo.GetRoleIdsString(), BearerDefaults.Manager);
            var refreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Id.ToString());

            await userService.ChangeUserValidateInfoExpiresDtAsync(id.Value);

            var tokenInfo = new UserTokenInfoDto(accessToken.Token, accessToken.Expire, refreshToken.Token, refreshToken.Expire);
            return Ok(tokenInfo);
        }
        return Forbid();
    }

    /// <summary>
    /// Changes the password of the current authenticated user.
    /// </summary>
    /// <param name="input">The password change request.</param>
    /// <returns>A result indicating whether the password was updated.</returns>
    [HttpPatch("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ChangePassword([FromBody] UserProfileChangePwdDto input)
        => Result(await userService.UpdatePasswordAsync(userContext.Id, input));

    /// <summary>
    /// Gets the validation info of the current authenticated user.
    /// </summary>
    /// <returns>The validation info for the current user.</returns>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserValidatedInfoDto>> GetUserValidatedInfoAsync()
    {
        var validatedInfo = await userService.GetUserValidatedInfoAsync(userContext.Id);
        logger.LogDebug("UserContext:{Id}", userContext.Id);
        return validatedInfo is null ? NotFound() : validatedInfo;
    }

    /// <summary>
    /// Gets the permissions owned by the current user from the requested permission set.
    /// </summary>
    /// <param name="id">The current user ID.</param>
    /// <param name="requestPermissions">The permissions to check.</param>
    /// <param name="userBelongsRoleIds">The comma-separated role IDs assigned to the user.</param>
    /// <returns>The matching permissions granted to the user.</returns>
    [HttpGet("{id}/permissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<string>>> GetCurrenUserPermissions([FromRoute] long id, [FromQuery] IEnumerable<string> requestPermissions, [FromQuery] string userBelongsRoleIds)
    {
        if (id != userContext.Id)
        {
            var userContextId = userContext.Id;
            logger.LogDebug("id={id},usercontextid={userContextId}", id, userContextId);
            return Forbid();
        }
        var result = await userService.GetPermissionsAsync(id, requestPermissions, userBelongsRoleIds);
        return result ?? [];
    }

    /// <summary>
    /// Gets the role and permission info of the current authenticated user.
    /// </summary>
    /// <returns>The current user's role and permission info.</returns>
    [HttpGet("userinfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserInfoDto>> GetUserInfoAsync()
    {
        var userInfo = await userService.GetUserInfoAsync(userContext);
        return userInfo is null ? NotFound() : userInfo;
    }

    /// <summary>
    /// Gets the profile details of the current authenticated user.
    /// </summary>
    /// <returns>The profile of the current user.</returns>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
    {
        var profile = await userService.GetProfileAsync(userContext.Id);
        return profile is null ? NotFound() : profile;
    }

    /// <summary>
    /// Updates the profile of the current authenticated user.
    /// </summary>
    /// <param name="input">The profile update request.</param>
    /// <returns>A result indicating whether the profile was updated.</returns>
    [HttpPatch("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ChangeProfileAsync([FromBody] UserProfileUpdationDto input)
        => Result(await userService.ChangeProfileAsync(userContext.Id, input));
}
