namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents a user.
/// </summary>
[Serializable]
public class UserDto : UserCreationAndUpdationDto
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation time.
    /// </summary>
    public DateTime CreateTime { get; set; }
}
