namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the shared fields used for creating and updating users.
/// </summary>
public abstract class UserCreationAndUpdationDto : InputDto
{
    /// <summary>
    /// Gets or sets the birthday.
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Gets or sets the department ID.
    /// </summary>
    public long DeptId { get; set; }

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
    /// Gets or sets the role IDs.
    /// </summary>
    public long[] RoleIds { get; set; } = [];

    /// <summary>
    /// Gets or sets the gender.
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the account is enabled.
    /// </summary>
    public bool Status { get; set; }
}
