namespace Adnc.Shared.Repository.DapperEntities;

/// <summary>
/// Operation logs
/// </summary>
public class OperationLog : Entity
{
    /// <summary>
    /// 控制器类名
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// 日志业务名称
    /// </summary>
    public string LogName { get; set; } = string.Empty;

    /// <summary>
    /// 日志类型
    /// </summary>
    public string LogType { get; set; } = string.Empty;

    /// <summary>
    /// 详细信息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 控制器方法
    /// </summary>
    public string Method { get; set; } = string.Empty;

    /// <summary>
    /// 是否操作成功
    /// </summary>
    public bool Succeed { get; set; }

    /// <summary>
    /// 操作用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string Account { get; set; } = string.Empty;

    /// <summary>
    /// 操作用户名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Ip
    /// </summary>
    public string RemoteIpAddress { get; set; } = string.Empty;

    /// <summary>
    /// 执行时间
    /// </summary>
    public int ExecutionTime { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
