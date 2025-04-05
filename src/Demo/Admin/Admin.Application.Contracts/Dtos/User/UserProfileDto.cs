namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 用户个人信息
/// </summary>
public class UserProfileDto : OutputDto
{
    /// <summary>
    /// 头像
    /// </summary>
    private string _avatar = string.Empty;

    /// <summary>
    /// 用户账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 性别
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 邮件地址
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间/注册时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 多个角色名称
    /// </summary>
    public string RoleNames { get; set; } = string.Empty;

    public string Avatar
    {
        set { _avatar = value; }
        get
        {
            if (_avatar.IsNullOrEmpty())
            {
                _avatar = "https://foruda.gitee.com/images/1723603502796844527/03cdca2a_716974.gif";
            }
            return _avatar;
        }
    }
}

public class UserProfileUpdationDto : InputDto
{
    /// <summary>
    /// 性别
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 姓名/昵称
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

public class UserProfileChangePwdDto : InputDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// 当前密码
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// 确认密码
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
