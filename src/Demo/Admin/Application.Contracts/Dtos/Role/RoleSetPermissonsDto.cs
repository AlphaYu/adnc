namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

/// <summary>
/// Represents the payload used to assign permissions to a role.
/// </summary>
public class RoleSetPermissonsDto : IDto
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets the permission menu IDs.
    /// </summary>
    public long[] Permissions { get; set; } = [];
}
