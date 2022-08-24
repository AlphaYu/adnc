namespace Adnc.Shared.Repository.MongoEntities;

/// <summary>
/// 登录日志
/// </summary>
public class LoginLog : MongoEntity
{
    public string Device { get; set; } = default!;

    public string Message { get; set; } = default!;

    public bool Succeed { get; set; } = default!;

    public int StatusCode { get; set; } = default!;

    public long? UserId { get; set; } = default!;

    public string Account { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string RemoteIpAddress { get; set; } = default!;

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? CreateTime { get; set; }
}