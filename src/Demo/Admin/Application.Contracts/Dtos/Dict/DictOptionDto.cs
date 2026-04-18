namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

/// <summary>
/// Represents a dictionary option collection.
/// </summary>
[Serializable]
public class DictOptionDto
{
    /// <summary>
    /// Gets or sets the dictionary name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dictionary code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dictionary data options.
    /// </summary>
    public DictDataOption[] DictDataList { get; set; } = [];

    /// <summary>
    /// Represents a single dictionary data option.
    /// </summary>
    [Serializable]
    public class DictDataOption
    {
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
    }
}
