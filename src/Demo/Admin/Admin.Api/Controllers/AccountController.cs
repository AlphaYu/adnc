namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 登录用户管理
/// </summary>
[Route($"{RouteConsts.AuthRoot}/session")]
[ApiController]
public class AccountController(IOptions<JWTOptions> jwtOptions, UserContext userContext, IUserService userService, ILogger<AccountController> logger)
    : AdncControllerBase
{
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"><see cref="UserLoginDto"/></param>
    /// <returns><see cref="UserTokenInfoDto"></see>/></returns>
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
    /// 注销
    /// </summary>
    /// <returns></returns>
    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> LogoutAsync()
        => Result(await userService.DeleteUserValidateInfoAsync(userContext.Id));

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="input"><see cref="UserRefreshTokenDto"/></param>
    /// <returns></returns>
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
    /// 登录用户修改密码
    /// </summary>
    /// <param name="input"><see cref="UserProfileChangePwdDto"/></param>
    /// <returns></returns>
    [HttpPatch("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ChangePassword([FromBody] UserProfileChangePwdDto input)
        => Result(await userService.UpdatePasswordAsync(userContext.Id, input));

    /// <summary>
    ///  获取登录用户认证信息
    /// </summary>
    /// <returns></returns>
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
    /// 获取登录用户是否拥有指定权限
    /// </summary>
    /// <param name="id">用户id</param>
    /// <param name="requestPermissions"></param>
    /// <param name="userBelongsRoleIds"></param>
    /// <returns></returns>
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
    /// 获取登录用户权限信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("userinfo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserInfoDto>> GetUserInfoAsync()
    {
        var userInfo = await userService.GetUserInfoAsync(userContext);
        return userInfo is null ? NotFound() : userInfo;
    }

    /// <summary>
    /// 获取登录用户个人中心信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfileAsync()
    {
        var profile = await userService.GetProfileAsync(userContext.Id);
        return profile is null ? NotFound() : profile;
    }

    /// <summary>
    /// 修改登录用户账户信息
    /// </summary>
    /// <returns></returns>
    [HttpPatch("profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ChangeProfileAsync([FromBody] UserProfileUpdationDto input)
        => Result(await userService.ChangeProfileAsync(userContext.Id, input));
}
