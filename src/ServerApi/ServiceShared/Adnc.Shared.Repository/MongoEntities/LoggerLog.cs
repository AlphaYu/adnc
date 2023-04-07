namespace Adnc.Shared.Repository.MongoEntities;

/// <summary>
/// ILogger日志
/// </summary>
//the driver would ignore any extra fields instead of throwing an exception
[BsonIgnoreExtraElements]
public class LoggerLog : MongoEntity
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Date { get; set; }

    public string Level { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string Logger { get; set; } = default!;

    public string Exception { get; set; } = default!;

    public int ThreadID { get; set; } = default!;

    public string ThreadName { get; set; } = default!;

    public int ProcessID { get; set; } = default!;

    public string ProcessName { get; set; } = default!;

    public NloglogProperty Properties { get; set; } = default!;
}

[BsonIgnoreExtraElements]
public class NloglogProperty
{
    public string TraceIdentifier { get; set; } = default!;

    public string ConnectionId { get; set; } = default!;

    public string EventId_Id { get; set; } = default!;

    public string EventId_Name { get; set; } = default!;

    public string EventId { get; set; } = default!;

    public string RemoteIpAddress { get; set; } = default!;

    public string BaseDir { get; set; } = default!;

    public string QueryUrl { get; set; } = default!;

    public string RequestMethod { get; set; } = default!;

    public string Controller { get; set; } = default!;

    public string Method { get; set; } = default!;

    public string FormContent { get; set; } = default!;

    public string QueryContent { get; set; } = default!;
}