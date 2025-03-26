using Adnc.Infra.Core.DependencyInjection;
using Adnc.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    /// <exception cref="NullReferenceException"></exception>
    public static async Task<QueryPageResult<TResult>> GetPagedLoginLogsBySqlAsync<TResult>(this IAdoQuerierRepository repository, QueryCondition condition, int offset, int rows)
        where TResult : notnull
    {
        var configuration = ServiceLocator.GetProvider().GetRequiredService<IConfiguration>();
        var dbTypeString = configuration[$"{NodeConsts.SysLogDb_DbType}"];
        var connectionString = configuration[$"{NodeConsts.SysLogDb_ConnectionString}"];
        using var _ = repository.ChangeOrSetDbConnection(connectionString, dbTypeString.ToUpper().ToEnum<DbTypes>());

        string countSql = $"select count(*) from login_log {condition.Where}";
        var total = await repository.QuerySingleAsync<int>(countSql, condition.Param);
        if (total == 0)
        {
            return new QueryPageResult<TResult>(total);
        }

        string sql = $"select * from login_log  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
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
    /// <exception cref="NullReferenceException"></exception>
    public static async Task<QueryPageResult<TResult>> GetPagedOperationLogsBySqlAsync<TResult>(this IAdoQuerierRepository repository, QueryCondition condition, int offset, int rows)
        where TResult : notnull
    {
        var configuration = ServiceLocator.GetProvider().GetRequiredService<IConfiguration>();
        var dbTypeString = configuration[$"{NodeConsts.SysLogDb_DbType}"];
        var connectionString = configuration[$"{NodeConsts.SysLogDb_ConnectionString}"];
        using var _ = repository.ChangeOrSetDbConnection(connectionString, dbTypeString.ToUpper().ToEnum<DbTypes>());

        string countSql = $"select count(*) from operation_log {condition.Where}";
        var total = await repository.QuerySingleAsync<int>(countSql, condition.Param);
        if (total == 0)
        {
            return new QueryPageResult<TResult>(total);
        }

        string sql = $"select * from operation_log  {condition.Where} {condition.OrderBy} limit {offset},{rows}";
        var result = await repository.QueryAsync<TResult>(sql, condition.Param);
        return new QueryPageResult<TResult>(total, result);
    }
}
