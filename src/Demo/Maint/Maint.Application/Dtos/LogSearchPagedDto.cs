namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// 日志检索条件
/// </summary>
public class LogSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// 日志范围开始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }

    /// <summary>
    /// 日志范围结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// 方法名
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// 设备
    /// </summary>
    public string? Device { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public string? Level { get; set; }
}