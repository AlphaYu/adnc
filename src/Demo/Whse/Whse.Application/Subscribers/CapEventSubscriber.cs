namespace Adnc.Demo.Whse.Application.Subscribers;

public sealed class CapEventSubscriber : ICapSubscribe
{
    private readonly IWarehouseAppService _wareHouserSrv;
    private readonly ILogger<CapEventSubscriber> _logger;
    private readonly IMessageTracker _tracker;

    public CapEventSubscriber(
        IWarehouseAppService wareHouserSrv,
        ILogger<CapEventSubscriber> logger,
        MessageTrackerFactory trackerFactory)
    {
        _wareHouserSrv = wareHouserSrv;
        _logger = logger;
        _tracker = trackerFactory.Create();
    }

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
            await _wareHouserSrv.BlockQtyAsync(eventDto, _tracker);
    }
}