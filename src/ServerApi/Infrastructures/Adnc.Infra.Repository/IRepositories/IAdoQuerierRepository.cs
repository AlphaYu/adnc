namespace Adnc.Infra.IRepositories;

public interface IAdoQuerierRepository : IAdoRepository
{
    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
    Task<IEnumerable<dynamic>?> QueryAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of results to return.</typeparam>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>
    /// A sequence of data of <typeparamref name="T"/>; if a basic type (int, string, etc) is queried then the data from the first column in assumed, otherwise an instance is
    /// created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
    /// </returns>
    Task<IEnumerable<T>?> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>

    Task<T> QueryFirstAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type of result to return.</typeparam>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<T> QuerySingleOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<dynamic> QueryFirstAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<dynamic> QueryFirstOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<dynamic> QuerySingleAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<dynamic> QuerySingleOrDefaultAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a query asynchronously using Task.
    /// </summary>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<IEnumerable<object>?> QueryAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<object> QueryFirstAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<object> QueryFirstOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<object> QuerySingleAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Execute a single-row query asynchronously using Task.
    /// </summary>
    /// <param name="type">The type to return.</param>
    /// <param name="sql">The SQL to execute for the query.</param>
    /// <param name="param">The parameters to pass, if any.</param>
    /// <param name="transaction">The transaction to use, if any.</param>
    /// <param name="commandTimeout">The command timeout (in seconds).</param>
    /// <param name="commandType">The type of command to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
    /// <param name="writeDb">if true writedb else readdb</param>
    Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 2 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 3 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 4 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 5 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 6 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with 7 input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TFirst">The first type in the recordset.</typeparam>
    /// <typeparam name="TSecond">The second type in the recordset.</typeparam>
    /// <typeparam name="TThird">The third type in the recordset.</typeparam>
    /// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
    /// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
    /// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
    /// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);

    /// <summary>
    /// Perform an asynchronous multi-mapping query with an arbitrary number of input types.
    /// This returns a single type, combined from the raw types via <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TReturn">The combined type to return.</typeparam>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="types">Array of types in the recordset.</param>
    /// <param name="map">The function to map row types to the return type.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="buffered">Whether to buffer the results in memory.</param>
    /// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <param name="writeDb">if true writedb else readdb</param>
    /// <returns>An enumerable of <typeparamref name="TReturn"/>.</returns>
    Task<IEnumerable<TReturn>?> QueryAsync<TReturn>(string sql, Type[] types, Func<object[], TReturn> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false);
}