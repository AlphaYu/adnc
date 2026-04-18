namespace Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

/// <summary>
/// Represents a system configuration.
/// </summary>
[Serializable]
public class SysConfigDto : SysConfigCreationDto
{
    /// <summary>
    /// Gets or sets the configuration ID.
    /// </summary>
    public long Id { get; set; }
}
