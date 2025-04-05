using Adnc.Demo.Remote.Event;

namespace Adnc.Demo.Whse.Application.Subscribers;

public sealed class CapEventSubscriber(IWarehouseService wareHouserSrv, MessageTrackerFactory trackerFactory) : ICapSubscribe
{
    private readonly IMessageTracker _tracker = trackerFactory.Create();

    /// <summary>
    /// 订阅订单创建事件
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderCreatedEvent))]
    public async Task ProcessOrderCreatedEvent(OrderCreatedEvent eventDto)
    {
        eventDto.EventTarget = MethodBase.GetCurrentMethod()?.GetMethodName() ?? string.Empty;
        var hasProcessed = await _tracker.HasProcessedAsync(eventDto);
        if (!hasProcessed)
        {
            await wareHouserSrv.BlockQtyAsync(eventDto, _tracker);
        }
    }
}
