namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

/// <summary>
/// Represents a role.
/// </summary>
[Serializable]
public class RoleDto : RoleCreationDto
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public long Id { get; set; }
}
