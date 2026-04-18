namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the payload used to sign in a user.
/// </summary>
public class UserLoginDto : InputDto
{
    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
