namespace Adnc.Demo.Admin.Api.Controllers;

/// <summary>
/// 用户管理
/// </summary>
[Route($"{RouteConsts.AdminRoot}/users")]
[ApiController]
public class UserController(IUserService userService) : AdncControllerBase
{
    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="input">用户信息</param>
    /// <returns></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.User.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] UserCreationDto input)
        => CreatedResult(await userService.CreateAsync(input));

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="input">用户信息</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [AdncAuthorize(PermissionConsts.User.Update)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] UserUpdationDto input)
        => Result(await userService.UpdateAsync(id, input));

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="ids">用户ID</param>
    /// <returns></returns>
    [HttpDelete("{ids}")]
    [AdncAuthorize(PermissionConsts.User.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteAsync([FromRoute] string ids)
    {
        var idArr = ids.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        return Result(await userService.DeleteAsync(idArr));
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="id"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPatch("{id}/password")]
    [AdncAuthorize(PermissionConsts.User.ResetPassword)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> ResetPasswordAsync([FromRoute] long id, string password)
        => Result(await userService.ResetPasswordAsync(id, password));

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns><see cref="UserDto"/></returns>
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
    /// 获取用户分页列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns><see cref="PageModelDto{UserDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.User.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<UserDto>>> GetPagedAsync([FromQuery] UserSearchPagedDto input)
        => await userService.GetPagedAsync(input);
}
