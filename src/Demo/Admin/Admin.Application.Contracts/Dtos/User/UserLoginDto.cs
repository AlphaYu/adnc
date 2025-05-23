namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 登录信息
/// </summary>
public class UserLoginDto : InputDto
{
    /// <summary>
    /// 账户
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
