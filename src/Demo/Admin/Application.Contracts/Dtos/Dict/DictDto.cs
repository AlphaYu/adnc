namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

/// <summary>
/// Represents a dictionary.
/// </summary>
[Serializable]
public class DictDto : DictCreationDto
{
    /// <summary>
    /// Gets or sets the dictionary ID.
    /// </summary>
    public long Id { get; set; }
}

/// <summary>
/// Represents a dictionary data entry.
/// </summary>
[Serializable]
public class DictDataDto : DictDataCreationDto
{
    /// <summary>
    /// Gets or sets the dictionary data entry ID.
    /// </summary>
    public long Id { get; set; }
}
