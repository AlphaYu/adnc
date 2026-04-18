namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

/// <summary>
/// Represents the payload used to create a role.
/// </summary>
public class RoleCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the role name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the role is enabled.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the data scope.
    /// </summary>
    public int DataScope { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int Ordinal { get; set; }
}
