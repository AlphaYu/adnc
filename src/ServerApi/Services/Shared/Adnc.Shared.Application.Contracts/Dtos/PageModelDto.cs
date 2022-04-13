namespace Adnc.Shared.Application.Contracts.Dtos;

[Serializable]
public class PageModelDto<T> : IDto
{
    private IReadOnlyList<T> _data = Array.Empty<T>();

    public PageModelDto()
    {
    }

    public PageModelDto(SearchPagedDto search)
        : this(search, default, default)
    {
    }

    public PageModelDto(SearchPagedDto search, IReadOnlyList<T> data, int count, dynamic xData = null)
        : this(search.PageIndex, search.PageSize, data, count)
    {
        this.XData = xData;
    }

    public PageModelDto(int pageIndex, int pageSize, IReadOnlyList<T> data, int count, dynamic xData = null)
    {
        this.PageIndex = pageIndex;
        this.PageSize = pageSize;
        this.TotalCount = count;
        this.Data = data;
        this.XData = xData;
    }

    public IReadOnlyList<T> Data
    {
        get => _data;
        set => _data = value ?? Array.Empty<T>();
    }

    public int RowsCount => _data.Count;

    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int PageCount => (this.RowsCount + this.PageSize - 1) / this.PageSize;

    public dynamic XData { get; set; }
}