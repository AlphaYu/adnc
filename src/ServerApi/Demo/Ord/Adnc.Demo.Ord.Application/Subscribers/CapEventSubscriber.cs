namespace Adnc.Demo.Ord.Application.EventSubscribers;

public sealed class CapEventSubscriber : ICapSubscribe
{
    private readonly IOrderAppService _orderSrv;
    private readonly ILogger<CapEventSubscriber> _logger;
    private readonly IMessageTracker _tracker;

    public CapEventSubscriber(
        IOrderAppService orderSrv,
        ILogger<CapEventSubscriber> logger,
        MessageTrackerFactory trackerFactory)
    {
        _orderSrv = orderSrv;
        _logger = logger;
        _tracker = trackerFactory.Create();
    }

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
            await _orderSrv.MarkCreatedStatusAsync(eventDto, _tracker);
    }
}