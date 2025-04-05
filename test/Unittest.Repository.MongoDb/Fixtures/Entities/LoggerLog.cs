using Adnc.Infra.Repository.EfCore.MongoDB;
using MongoDB.Bson.Serialization.Attributes;

namespace Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures.Entities;

/// <summary>
/// ILogger日志
/// </summary>
//the driver would ignore any extra fields instead of throwing an exception
[BsonIgnoreExtraElements]
public class LoggerLog : MongoEntity
{
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Date { get; set; }

    public string Level { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Logger { get; set; } = string.Empty;

    public string Exception { get; set; } = string.Empty;

    public int ThreadId { get; set; }

    public string ThreadName { get; set; } = string.Empty;

    public int ProcessID { get; set; }

    public string ProcessName { get; set; }

    public NloglogProperty Properties { get; set; }
}

[BsonIgnoreExtraElements]
public class NloglogProperty
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
