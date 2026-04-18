namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

/// <summary>
/// Represents the paging and filtering criteria used to search dictionary data entries.
/// </summary>
public class DictDataSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// Gets or sets the dictionary code filter.
    /// </summary>
    public string? DictCode { get; set; }
}
