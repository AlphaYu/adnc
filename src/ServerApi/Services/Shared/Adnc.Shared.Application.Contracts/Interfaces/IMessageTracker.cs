using Adnc.Shared.Application.Contracts.Enums;

namespace Adnc.Shared.Application.Contracts.Interfaces;

public interface IMessageTracker
{
    TrackerKind Kind { get; }
    Task<bool> HasProcessedAsync(long eventId, string trackerName);
    Task MarkAsProcessedAsync(long eventId, string trackerName);
}