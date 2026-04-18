namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the payload used to create a user.
/// </summary>
public class UserCreationDto : UserCreationAndUpdationDto
{
    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    public string Account { get; set; } = string.Empty;
}
