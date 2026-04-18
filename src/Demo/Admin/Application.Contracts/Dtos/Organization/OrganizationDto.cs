namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

/// <summary>
/// Represents an organization.
/// </summary>
[Serializable]
public class OrganizationDto : OrganizationCreationDto
{
    /// <summary>
    /// Gets or sets the organization ID.
    /// </summary>
    public long Id { get; set; }
}
