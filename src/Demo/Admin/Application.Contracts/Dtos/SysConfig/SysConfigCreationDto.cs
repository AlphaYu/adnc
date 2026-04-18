namespace Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

/// <summary>
/// Represents the payload used to create a system configuration.
/// </summary>
public class SysConfigCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the configuration key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configuration name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configuration value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remark.
    /// </summary>
    public string Remark { get; set; } = string.Empty;
}
