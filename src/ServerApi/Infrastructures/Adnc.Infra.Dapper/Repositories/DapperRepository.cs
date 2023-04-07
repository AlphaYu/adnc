namespace Adnc.Infra.Dapper.Repositories;

public sealed class DapperRepository : IAdoExecuterWithQuerierRepository
{
    internal IDbConnection? DbConnection { get; private set; }

    public void ChangeOrSetDbConnection(IDbConnection dbConnection)
    {
        if (dbConnection is null)
            throw new ArgumentNullException(nameof(dbConnection));
        DbConnection = dbConnection;
    }

    public void ChangeOrSetDbConnection(string connectionString, DbTypes dbType)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        DbConnection = dbType switch
        {
            DbTypes.MYSQL => new MySqlConnection(connectionString),
            //DbTypes.SQLSERVER => new MySqlConnection(connectionString),
            //DbTypes.ORACLE => new MySqlConnection(connectionString),
            _ => throw new NotImplementedException()
        };
    }

    public bool HasDbConnection() => DbConnection is not null;

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);

    public async Task<IDataReader> ExecuteReaderAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    => await SqlMapper.ExecuteReaderAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);

    public async Task<object> ExecuteScalarAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteScalarAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);

    public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteScalarAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);

    public async Task<IEnumerable<dynamic>?> QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<T>?> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<object>?> QueryAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync(DbConnection, type, sql, param, transaction, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(DbConnection, sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<IEnumerable<TReturn>?> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryAsync<TReturn>(DbConnection, sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result.Any() ? result : null;
    }

    public async Task<T> QueryFirstAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QueryFirstAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QueryFirstAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstAsync(DbConnection, type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstOrDefaultAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QueryFirstOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstOrDefaultAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QueryFirstOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QueryFirstOrDefaultAsync(DbConnection, type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QuerySingleAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QuerySingleAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleAsync(DbConnection, type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleOrDefaultAsync<T>(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QuerySingleOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleOrDefaultAsync(DbConnection, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb) sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        var result = await SqlMapper.QuerySingleOrDefaultAsync(DbConnection, type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }
}