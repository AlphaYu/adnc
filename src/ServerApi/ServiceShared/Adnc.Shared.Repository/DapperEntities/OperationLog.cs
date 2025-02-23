namespace Adnc.Shared.Repository.DapperEntities;

/// <summary>
/// Operation logs
/// </summary>
public class OperationLog : Entity
{
    public string ClassName { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public string LogName { get; set; } = string.Empty;
    public string LogType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public bool Succeed { get; set; }
    public long UserId { get; set; }
    public string Account { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string RemoteIpAddress { get; set; } = string.Empty;
}