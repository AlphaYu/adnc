namespace Adnc.Shared.Entities;

/// <summary>
/// 事件跟踪/处理信息
/// </summary>
public class EventTracker : EfBasicAuditEntity
{
    public long EventId { get; set; }

    [Required]
    [MaxLength(50)]
    public string TrackerName { get; set; }
}