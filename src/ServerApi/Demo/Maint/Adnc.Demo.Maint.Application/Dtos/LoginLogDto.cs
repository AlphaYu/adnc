namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// 登录日志
/// </summary>
public class LoginLogDto : MongoDto
{
    public string Device { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool Succeed { get; set; }

    public int StatusCode { get; set; }

    public long? UserId { get; set; }

    public string Account { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string RemoteIpAddress { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; }
}