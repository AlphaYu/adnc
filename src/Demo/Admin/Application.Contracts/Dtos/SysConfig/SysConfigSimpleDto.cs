namespace Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

/// <summary>
/// Represents a lightweight system configuration.
/// </summary>
[Serializable]
public class SysConfigSimpleDto
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
}
