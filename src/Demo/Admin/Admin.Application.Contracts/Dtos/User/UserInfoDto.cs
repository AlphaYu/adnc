namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 用户信息
/// </summary>
public class UserInfoDto : OutputDto
{
    /// <summary>
    /// 头像
    /// </summary>
    private string _avatar = string.Empty;

    /// <summary>
    ///  用户名
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    ///  姓名/昵称
    /// </summary>
    public string Name { get; set; } = string.Empty;

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

    /// <summary>
    /// 角色代码集合
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// 权限集合
    /// </summary>
    public string[] Perms { get; set; } = [];
}
