namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Notice;

/// <summary>
/// Represents the paging and filtering criteria used to search notices.
/// </summary>
public class NoticeSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// Gets or sets the title filter.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the publish status filter.
    /// </summary>
    public int? PublishStatus { get; set; }

    /// <summary>
    /// Gets or sets the read-state filter.
    /// </summary>
    public int? IsRead { get; set; }
}
