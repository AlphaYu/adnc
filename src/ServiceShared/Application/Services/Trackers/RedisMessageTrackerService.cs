using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Tracker;

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

    public async Task<bool> HasProcessedAsync(BaseEvent eventModel)
    {
        return await HasProcessedAsync(eventModel.Id, eventModel.EventTarget);
    }

    public async Task MarkAsProcessedAsync(BaseEvent eventModel)
    {
        await MarkAsProcessedAsync(eventModel.Id, eventModel.EventTarget);
    }
}
