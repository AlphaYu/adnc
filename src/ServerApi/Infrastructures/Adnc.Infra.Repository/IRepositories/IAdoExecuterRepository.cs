namespace Adnc.Infra.IRepositories;

public interface IAdoExecuterRepository : IAdoRepository
{
    /// <summary>
    /// Execute parameterized SQL and return an <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>An <see cref="IDataReader"/> that can be used to iterate over the results of the SQL query.</returns>
    /// <remarks>
    /// This is typically used when the results of a query are not processed by Dapper, for example, used to fill a <see cref="DataTable"/>
    /// or <see cref="T:DataSet"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// DataTable table = new DataTable("MyTable");
    /// using (var reader = ExecuteReader(cnn, sql, param))
    /// {
    ///     table.Load(reader);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    Task<IDataReader> ExecuteReaderAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <see cref="object"/>.</returns>
    Task<object> ExecuteScalarAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Execute parameterized SQL that selects a single value.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="sql">The SQL to execute.</param>
    /// <param name="param">The parameters to use for this command.</param>
    /// <param name="transaction">The transaction to use for this command.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The first cell returned, as <typeparamref name="T"/>.</returns>
    Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

    /// <summary>
    /// Execute a command asynchronously using Task.
    /// </summary>
    /// <param name="sql">The SQL to execute for this query.</param>
    /// <param name="param">The parameters to use for this query.</param>
    /// <param name="transaction">The transaction to use for this query.</param>
    /// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
    /// <param name="commandType">Is it a stored proc or a batch?</param>
    /// <returns>The number of rows affected.</returns>
    Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
}