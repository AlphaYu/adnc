namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// Nlog日志
/// </summary>
public class NlogLogDto : MongoDto
{
    public DateTime Date { get; set; }

    public string Level { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Logger { get; set; } = string.Empty;

    public string Exception { get; set; } = string.Empty;

    public int ThreadID { get; set; }

    public string ThreadName { get; set; } = string.Empty;

    public int ProcessID { get; set; }

    public string ProcessName { get; set; } = string.Empty;

    public NlogLogPropertyDto Properties { get; set; } = new NlogLogPropertyDto();
}

public class NlogLogPropertyDto
{
    public string TraceIdentifier { get; set; } = string.Empty;

    public string ConnectionId { get; set; } = string.Empty;

    public string EventId_Id { get; set; } = string.Empty;

    public string EventId_Name { get; set; } = string.Empty;

    public string EventId { get; set; } = string.Empty;

    public string RemoteIpAddress { get; set; } = string.Empty;

    public string BaseDir { get; set; } = string.Empty;

    public string QueryUrl { get; set; } = string.Empty;

    public string RequestMethod { get; set; } = string.Empty;

    public string Controller { get; set; } = string.Empty;

    public string Method { get; set; } = string.Empty;

    public string FormContent { get; set; } = string.Empty;

    public string QueryContent { get; set; } = string.Empty;
}