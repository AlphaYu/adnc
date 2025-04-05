namespace Adnc.Shared.Application.Services.Trackers;

public sealed class MessageTrackerFactory(IEnumerable<IMessageTracker> trackers)
{
    public IEnumerable<IMessageTracker> _trackers = trackers;

    public IMessageTracker Create(TrackerKind kind = TrackerKind.Db)
    {
        if (_trackers.IsNullOrEmpty())
        {
            return new NullMessageTrackerService();
        }

        return _trackers.FirstOrDefault(x => x.Kind == kind) ?? new NullMessageTrackerService();
    }
}
