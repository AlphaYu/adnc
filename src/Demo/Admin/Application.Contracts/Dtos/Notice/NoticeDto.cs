namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Notice;

/// <summary>
/// Represents a notice.
/// </summary>
[Serializable]
public class NoticeDto
{
    /// <summary>
    /// Gets or sets the notice ID.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the notice title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the notice content.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the notice type.
    /// </summary>
    public int? Type { get; set; }

    /// <summary>
    /// Gets or sets the publisher ID.
    /// </summary>
    public long? PublisherId { get; set; }

    /// <summary>
    /// Gets or sets the priority (0: low, 1: medium, 2: high).
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// Gets or sets the target type (0: all, 1: specified users).
    /// </summary>
    public int? TargetType { get; set; }

    /// <summary>
    /// Gets or sets the publish status (0: unpublished, 1: published, 2: revoked).
    /// </summary>
    public int? PublishStatus { get; set; }

    /// <summary>
    /// Gets or sets the publish time.
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// Gets or sets the revoke time.
    /// </summary>
    public DateTime? RevokeTime { get; set; }
}
