namespace Adnc.Infra.EventBus.Tracker;

public interface IMessageTracker
{
    TrackerKind Kind { get; }
    Task<bool> HasProcessedAsync(long eventId, string trackerName);
    Task MarkAsProcessedAsync(long eventId, string trackerName);
    Task<bool> HasProcessedAsync(BaseEvent eventModel);
    Task MarkAsProcessedAsync(BaseEvent eventModel);
}
