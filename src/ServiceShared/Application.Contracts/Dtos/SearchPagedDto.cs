namespace Adnc.Shared.Application.Contracts.Dtos;

/// <summary>
/// Base class for query criteria
/// </summary>
public class SearchPagedDto : IDto
{
    private int _pageIndex;
    private int _pageSize;

    /// <summary>
    /// Page index
    /// </summary>
    public int PageIndex
    {
        get => _pageIndex < 1 ? 1 : _pageIndex;
        set => _pageIndex = value;
    }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize
    {
        get
        {
            if (_pageSize < 5)
            {
                _pageSize = 5;
            }

            if (_pageSize > 100)
            {
                _pageSize = 100;
            }

            return _pageSize;
        }
        set => _pageSize = value;
    }

    /// <summary>
    /// Keywords
    /// </summary>
    public string? Keywords { get; set; }

    /// <summary>
    /// Creation time
    /// </summary>
    public DateTime[]? CreateTime { get; set; }
}
