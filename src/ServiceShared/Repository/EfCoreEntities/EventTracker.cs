namespace Adnc.Shared.Repository.EfCoreEntities;

/// <summary>
/// Event tracking/processing information
/// </summary>
/// <remarks>
/// EventId and TrackerName require a composite unique index
/// CREATE UNIQUE INDEX UK_EventId_TrackerNam ON EventTracker(EventId, TrackerName);
/// </remarks>
public class EventTracker : EfBasicAuditEntity
{
    public long EventId { get; set; }

    public string TrackerName { get; set; } = string.Empty;
}
