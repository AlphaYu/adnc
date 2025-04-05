namespace Adnc.Demo.Cust.Api.Repository;

public static class RepositoryExtension
{
    /// <summary>
    ///  通过sql查询客户分页信息
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="repository"></param>
    /// <param name="condition"></param>
    /// <param name="offset"></param>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<QueryPageResult<TResult>> GetPagedCustmersBySqlAsync<TResult>(this IEfRepository<Customer> repository, QueryCondition condition, int offset, int rows)
        where TResult : notnull
    {
        var countSql = $"select count(*) from cust_customer {condition.Where}";
        var total = await repository.AdoQuerier.QuerySingleAsync<int>(countSql, condition.Param);
        if (total == 0)
        {
            return new QueryPageResult<TResult>(total);
        }

        var sql = $"select * from cust_customer  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
        var result = await repository.AdoQuerier.QueryAsync<TResult>(sql, condition.Param);
        return new QueryPageResult<TResult>(total, result);
    }
}
