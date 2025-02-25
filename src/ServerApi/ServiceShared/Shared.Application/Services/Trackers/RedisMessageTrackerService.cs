namespace Adnc.Shared.Application.Services.Trackers;

public class RedisMessageTrackerService : IMessageTracker
{
    public TrackerKind Kind => TrackerKind.Redis;

    public async Task<bool> HasProcessedAsync(long eventId, string trackerName)
    {
        return await Task.FromResult(false);
    }

    public async Task MarkAsProcessedAsync(long eventId, string trackerName)
    {
        await Task.CompletedTask;
    }
}
