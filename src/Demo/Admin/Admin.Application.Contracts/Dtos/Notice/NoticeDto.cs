namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 通知
/// </summary>
[Serializable]
public class NoticeDto
{
    public long Id { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 通知内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型
    /// </summary>
    public int? Type { get; set; }

    /// <summary>
    /// 发布人
    /// </summary>
    public long? PublisherId { get; set; }

    /// <summary>
    /// 优先级(0-低 1-中 2-高)
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 目标类型(0-全体 1-指定)
    /// </summary>
    public int? TargetType { get; set; }

    /// <summary>
    /// 发布状态(0-未发布 1已发布 2已撤回)
    /// </summary>
    public int? PublishStatus { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 撤回时间
    /// </summary>
    public DateTime? RevokeTime { get; set; }
}
