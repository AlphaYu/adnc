namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization;

/// <summary>
/// Represents the payload used to create an organization.
/// </summary>
public class OrganizationCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the parent organization ID.
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// Gets or sets the organization code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the organization name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the organization is enabled.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int Ordinal { get; set; }
}
