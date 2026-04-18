namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

/// <summary>
/// Represents an organization tree node.
/// </summary>
[Serializable]
public class OrganizationTreeDto : OrganizationDto
{
    /// <summary>
    /// Gets or sets the child organizations.
    /// </summary>
    public List<OrganizationTreeDto> Children { get; set; } = [];
}
