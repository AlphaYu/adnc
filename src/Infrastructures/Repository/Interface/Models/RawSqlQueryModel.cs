namespace Adnc.Infra.Repository;

public class QueryCondition(string? where, string? orderBy, string? groupBy, object? param)
{
    public string? Where { get; init; } = where;
    public string? OrderBy { get; init; } = orderBy;
    public string? GroupBy { get; init; } = groupBy;
    public object? Param { get; init; } = param;
}

public class QueryPageResult<TResult>
    where TResult : notnull
{
    public QueryPageResult(int total)
    {
        TotalCount = total;
        Content = [];
    }

    public QueryPageResult(int total, IEnumerable<TResult>? result)
    {
        TotalCount = total;
        Content = result ?? [];
    }

    public int TotalCount { get; init; }
    public IEnumerable<TResult> Content { get; init; }
}
