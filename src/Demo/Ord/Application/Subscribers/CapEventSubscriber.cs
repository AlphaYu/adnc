using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Ord.Application.EventSubscribers;

public sealed class CapEventSubscriber(IOrderService orderSrv, MessageTrackerFactory trackerFactory) : ICapSubscribe
{
    private readonly IMessageTracker _tracker = trackerFactory.Create();

    /// <summary>
    /// 订阅库存锁定事件
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
