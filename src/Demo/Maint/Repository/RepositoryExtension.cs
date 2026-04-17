namespace Adnc.Demo.Maint.Repository;

public static class RepositoryExtension
{
    /// <summary>
    ///  通过sql查询登录日志分页信息
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="repository"></param>
    /// <param name="condition"></param>
    /// <param name="offset"></param>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task<QueryPageResult<TResult>> GetPagedLoginLogsBySqlAsync<TResult>(this IAdoQuerierRepository repository, QueryCondition condition, int offset, int rows)
        where TResult : notnull
    {
        var configuration = ServiceLocator.GetProvider().GetRequiredService<IConfiguration>();
        var (connectionString, dbType) = configuration.GetDbConnectionInfo(NodeConsts.SysLogDb);
        using var _ = repository.CreateDbConnection(connectionString, dbType);

        var countSql = $"select count(*) from login_log {condition.Where}";
        var total = await repository.QuerySingleAsync<int>(countSql, condition.Param);
        if (total == 0)
        {
            return new QueryPageResult<TResult>(total);
        }
        var sql = $"select * from login_log  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
        var result = await repository.QueryAsync<TResult>(sql, condition.Param);

        return new QueryPageResult<TResult>(total, result);
    }

    /// <summary>
    ///  通过sql查询登录日志分页信息
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="repository"></param>
    /// <param name="condition"></param>
    /// <param name="offset"></param>
    /// <param name="rows"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static async Task<QueryPageResult<TResult>> GetPagedOperationLogsBySqlAsync<TResult>(this IAdoQuerierRepository repository, QueryCondition condition, int offset, int rows)
        where TResult : notnull
    {
        var configuration = ServiceLocator.GetProvider().GetRequiredService<IConfiguration>();
        var (connectionString, dbType) = configuration.GetDbConnectionInfo(NodeConsts.SysLogDb);
        using var _ = repository.CreateDbConnection(connectionString, dbType);

        var countSql = $"select count(*) from operation_log {condition.Where}";
        var total = await repository.QuerySingleAsync<int>(countSql, condition.Param);
        if (total == 0)
        {
            return new QueryPageResult<TResult>(total);
        }
        var sql = $"select * from operation_log  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
        var result = await repository.QueryAsync<TResult>(sql, condition.Param);

        return new QueryPageResult<TResult>(total, result);
    }
}
