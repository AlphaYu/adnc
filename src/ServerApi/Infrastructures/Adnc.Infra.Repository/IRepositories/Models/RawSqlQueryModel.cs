namespace Adnc.Infra.IRepositories
{
    public class QueryCondition
    {
        public QueryCondition(string? where, string? orderBy, string? groupBy, object? param)
        {
            Where = where;
            OrderBy = orderBy;
            GroupBy = groupBy;
            Param = param;
        }

        public string? Where { get; init; }
        public string? OrderBy { get; init; }
        public string? GroupBy { get; init; }
        public object? Param { get; init; }
    }

    public class QueryPageResult<TResult>
        where TResult : notnull
    {
        public QueryPageResult(int total)
        {
            TotalCount = total;
            Content = Enumerable.Empty<TResult>();
        }

        public QueryPageResult(int total, IEnumerable<TResult>? result)
        {
            TotalCount = total;
            Content = result ?? Enumerable.Empty<TResult>();
        }

        public int TotalCount { get; init; }
        public IEnumerable<TResult> Content { get; init; }
    }
}
