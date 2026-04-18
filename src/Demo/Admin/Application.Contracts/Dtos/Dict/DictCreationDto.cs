namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

/// <summary>
/// Represents the payload used to create a dictionary.
/// </summary>
public class DictCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the dictionary code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dictionary name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remark.
    /// </summary>
    public string Remark { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the dictionary is enabled.
    /// </summary>
    public bool Status { get; set; }
}

/// <summary>
/// Represents the payload used to create a dictionary data entry.
/// </summary>
public class DictDataCreationDto : InputDto
{
    /// <summary>
    /// Gets or sets the dictionary code.
    /// </summary>
    public string DictCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the display label.
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stored value.
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tag type.
    /// </summary>
    public string TagType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the entry is enabled.
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the display order.
    /// </summary>
    public int Ordinal { get; set; }
}
