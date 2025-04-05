namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public class PageModelDto<T> : IDto
    where T : notnull
{
    private IReadOnlyList<T> _data = [];

    public PageModelDto()
    {
    }

    public PageModelDto(SearchPagedDto search)
        : this(search, [], default)
    {
    }

    public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic? xData = null)
        : this(search.PageIndex, search.PageSize, data, count)
    {
        XData = xData ?? new object();
    }

    public PageModelDto(int pageIndex, int pageSize, IReadOnlyList<T> data, int count, dynamic? xData = null)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Total = count;
        List = data;
        XData = xData ?? new object();
    }

    public IReadOnlyList<T> List
    {
        get => _data;
        set => _data = value ?? [];
    }

    public int RowsCount => _data.Count;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public int PageCount => (RowsCount + PageSize - 1) / PageSize;

    public dynamic XData { get; set; } = new object();
}
