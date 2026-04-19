using Adnc.Demo.Remote.Event;
using Adnc.Infra.EventBus.Tracker;

namespace Adnc.Demo.Ord.Application.EventSubscribers;

public sealed class WarehouseQtyBlockedEventSubscriber(IOrderService orderSrv, MessageTrackerFactory trackerFactory) : ICapSubscribe
{
    private readonly IMessageTracker _tracker = trackerFactory.Create();

    /// <summary>
    /// Subscribe to inventory reservation events
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(WarehouseQtyBlockedEvent))]
    public async Task ProcessWarehouseQtyBlockedEvent(WarehouseQtyBlockedEvent eventDto)
    {
        eventDto.EventTarget = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty;
        var hasProcessed = await _tracker.HasProcessedAsync(eventDto);
        if (!hasProcessed)
        {
            await orderSrv.MarkCreatedStatusAsync(eventDto, _tracker);
        }
    }
}
