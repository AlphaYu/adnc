namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

/// <summary>
/// Represents a menu-to-role relation.
/// </summary>
[Serializable]
public class RoleMenuRelationDto : IDto
{
    /// <summary>
    /// Gets or sets the menu ID.
    /// </summary>
    public long MenuId { get; set; }

    /// <summary>
    /// Gets or sets the role ID.
    /// </summary>
    public long RoleId { get; set; }
}
