namespace Adnc.Maint.Application.Contracts.Dtos;

/// <summary>
/// 登录日志
/// </summary>
public class LoginLogDto : MongoDto
{
    public string Device { get; set; }

    public string Message { get; set; }

    public bool Succeed { get; set; }

    public int StatusCode { get; set; }

    public long? UserId { get; set; }

    public string Account { get; set; }

    public string UserName { get; set; }

    public string RemoteIpAddress { get; set; }

    public DateTime? CreateTime { get; set; }
}