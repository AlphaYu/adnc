namespace Adnc.Infra.IRepositories;

[Serializable]
public class PagedModel<T> where T : class
{
    private int _pageIndex = 1;
    private int _pageSize = 10;
    private long _totalCount = 0;
    private IReadOnlyList<T> _data = Array.Empty<T>();

    public static PagedModel<T> Empty => new();

    public IReadOnlyList<T> Data
    {
        get => _data;
        set => _data = value ?? Array.Empty<T>();
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : 10;
    }

    public long TotalCount
    {
        get => _totalCount;
        set => _totalCount = value > 0 ? value : 0;
    }

    public int PageCount => (int)((_totalCount + _pageSize - 1) / _pageSize);

    public int Count => Data.Count;

    public T this[int index] => Data[index];
}