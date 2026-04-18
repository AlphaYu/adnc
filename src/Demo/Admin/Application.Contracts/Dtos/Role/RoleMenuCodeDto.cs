namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

/// <summary>
/// Represents a role and its permission codes.
/// </summary>
[Serializable]
public class RoleMenuCodeDto : IDto
{
    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public long RoleId { get; set; }

    /// <summary>
    /// Gets or sets the permission codes.
    /// </summary>
    public string[] Perms { get; set; } = [];
}
