namespace Adnc.Shared.Repository.DapperEntities;

/// <summary>
/// Operation logs
/// </summary>
public class OperationLog : Entity
{
    /// <summary>
    /// Controller class name
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Log business name
    /// </summary>
    public string LogName { get; set; } = string.Empty;

    /// <summary>
    /// Log type
    /// </summary>
    public string LogType { get; set; } = string.Empty;

    /// <summary>
    /// Details
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Controller method
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// Whether the operation succeeded
    /// </summary>
    public bool Succeed { get; set; }

    /// <summary>
    /// Operator user ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Account
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// Operator user name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ip
    /// </summary>
    public string RemoteIpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Execution time
    /// </summary>
    public int ExecutionTime { get; set; }

    /// <summary>
    /// Operation time
    /// </summary>
    public DateTime CreateTime { get; set; }
}
