namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the current user's profile.
/// </summary>
public class UserProfileDto : OutputDto
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
    /// Gets or sets the department name.
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile phone number.
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Gets or sets the role names.
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

/// <summary>
/// Represents the payload used to update the current user's profile.
/// </summary>
public class UserProfileUpdationDto : InputDto
{
    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// Gets or sets the display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Represents the payload used to change the current user's password.
/// </summary>
public class UserProfileChangePwdDto : InputDto
{
    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confirmation password.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
