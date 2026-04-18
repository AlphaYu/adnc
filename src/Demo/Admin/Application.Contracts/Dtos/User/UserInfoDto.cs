namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the authenticated user's role and permission info.
/// </summary>
public class UserInfoDto : OutputDto
{
    /// <summary>
    /// Stores the avatar URL.
    /// </summary>
    private string _avatar = string.Empty;

    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name.
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
    /// Gets or sets the role codes.
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// Gets or sets the permissions.
    /// </summary>
    public string[] Perms { get; set; } = [];
}
