namespace Adnc.Shared.Application.Contracts.Interfaces;

public static class MessageTrackerExtension
{
    public static async Task<bool> HasProcessedAsync(this IMessageTracker tracker, Rpc.Event.EventEntity eventDto)
    {
        return await tracker.HasProcessedAsync(eventDto.Id, eventDto.EventTarget);
    }

    public static async Task MarkAsProcessedAsync(this IMessageTracker tracker, Rpc.Event.EventEntity eventDto)
    {
        await tracker.MarkAsProcessedAsync(eventDto.Id, eventDto.EventTarget);
    }
}
