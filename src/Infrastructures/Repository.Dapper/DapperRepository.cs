namespace Adnc.Infra.Repository.Dapper;

public sealed class DapperRepository : IAdoExecuterWithQuerierRepository
{
    private IDbConnection? _dbConnection;

    public void ChangeOrSetDbConnection(IDbConnection dbConnection)
    {
        ArgumentNullException.ThrowIfNull(dbConnection);
        _dbConnection = dbConnection;
    }

    public IDbConnection ChangeOrSetDbConnection(string connectionString, DbTypes dbType)
    {
        return CreateDbConnection(connectionString, dbType);
    }

    public IDbConnection CreateDbConnection(string connectionString, DbTypes dbType)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString, nameof(connectionString));
        ArgumentNullException.ThrowIfNull(dbType, nameof(dbType));

        return _dbConnection = dbType switch
        {
            DbTypes.MYSQL => new MySqlConnector.MySqlConnection(connectionString),
            DbTypes.SQLSERVER => new Microsoft.Data.SqlClient.SqlConnection(connectionString),
            DbTypes.ORACLE => new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString),
            _ => throw new NotImplementedException()
        };
    }

    public bool HasDbConnection() => _dbConnection is not null;

    public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);

    public async Task<IDataReader> ExecuteReaderAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
    => await SqlMapper.ExecuteReaderAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);

    public async Task<object?> ExecuteScalarAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteScalarAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);

    public async Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
     => await SqlMapper.ExecuteScalarAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);

    public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<object>> QueryAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync(GetDbConnection(), type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(GetDbConnection(), sql, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryAsync<TReturn>(GetDbConnection(), sql, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
        return result;
    }

    public async Task<T> QueryFirstAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QueryFirstAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QueryFirstAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstAsync(GetDbConnection(), type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstOrDefaultAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic?> QueryFirstOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstOrDefaultAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object?> QueryFirstOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QueryFirstOrDefaultAsync(GetDbConnection(), type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic> QuerySingleAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object> QuerySingleAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleAsync(GetDbConnection(), type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<T?> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleOrDefaultAsync<T>(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<dynamic?> QuerySingleOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleOrDefaultAsync(GetDbConnection(), sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    public async Task<object?> QuerySingleOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
        {
            sql = string.Concat(sql, " -- ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        }

        var result = await SqlMapper.QuerySingleOrDefaultAsync(GetDbConnection(), type, sql, param, transaction, commandTimeout, commandType);
        return result;
    }

    internal IDbConnection GetDbConnection()
    {
        ArgumentNullException.ThrowIfNull(_dbConnection, nameof(_dbConnection));
        return _dbConnection;
    }
}
