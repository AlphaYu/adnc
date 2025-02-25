namespace Adnc.Demo.Maint.Application.Dtos;

/// <summary>
/// 系统通知
/// </summary>
public class NoticeDto : OutputBaseAuditDto
{
    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 类型
    /// </summary>
    public int Type { get; set; }
}