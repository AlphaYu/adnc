namespace Adnc.Demo.Usr.Api.Controllers;

/// <summary>
/// 认证管理
/// </summary>
[Route($"{RouteConsts.AuthRoot}/session")]
[ApiController]
public class AccountController(IOptions<JWTOptions> jwtOptions, UserContext userContext, IUserAppService userService, ILogger<AccountController> logger) 
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
    public async Task<ActionResult> LogoutAsync() => Result(await userService.DeleteUserValidateInfoAsync(userContext.Id));

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
                return Forbid();

            var validatedInfo = await userService.GetUserValidatedInfoAsync(id.Value);
            if (validatedInfo is null)
                return Forbid();

            var jti = JwtTokenHelper.GetClaimFromRefeshToken(jwtOptions.Value, input.RefreshToken, JwtRegisteredClaimNames.Jti);
            if (jti is null || jti.Value != validatedInfo.ValidationVersion)
                return Forbid();

            var accessToken = JwtTokenHelper.CreateAccessToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Account, validatedInfo.Id.ToString(), validatedInfo.Name, validatedInfo.GetRoleIdsString(), BearerDefaults.Manager);
            var refreshToken = JwtTokenHelper.CreateRefreshToken(jwtOptions.Value, validatedInfo.ValidationVersion, validatedInfo.Id.ToString());

            await userService.ChangeUserValidateInfoExpiresDtAsync(id.Value);

            var tokenInfo = new UserTokenInfoDto(accessToken.Token, accessToken.Expire, refreshToken.Token, refreshToken.Expire);
            return Ok(tokenInfo);
        }
        return Forbid();
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="input"><see cref="UserChangePwdDto"/></param>
    /// <returns></returns>
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ChangePassword([FromBody] UserProfileChangePwdDto input) => Result(await userService.UpdatePasswordAsync(userContext.Id, input));

    /// <summary>
    ///  获取认证信息
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserValidatedInfoDto>> GetUserValidatedInfoAsync()
    {
        var result = await userService.GetUserValidatedInfoAsync(userContext.Id);
        logger.LogDebug($"UserContext:{userContext.Id}");
        if (result is null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// 获取当前用户是否拥有指定权限
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
            logger.LogDebug($"id={id},usercontextid={userContext.Id}");
            return Forbid();
        }
        var result = await userService.GetPermissionsAsync(id, requestPermissions, userBelongsRoleIds);
        return result ?? new List<string>();
    }

    /// <summary>
    /// 获取登录用户个人中心信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserProfileDto?>> GetUserProfileAsync() => await userService.GetProfileAsync(userContext.Id);

    /// <summary>
    /// 修改当前账户信息
    /// </summary>
    /// <returns></returns>
    [HttpPut("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeUserProfileAsync([FromBody]UserProfileUpdationDto input) => Result(await userService.ChangeProfileAsync(userContext.Id, input));
}