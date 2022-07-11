namespace Adnc.Cus.Application.EventSubscribers;

public sealed partial class CapEventSubscriber : ICapSubscribe
{
    private readonly ICustomerAppService _customerSrv;
    private readonly ILogger<CapEventSubscriber> _logger;
    private readonly IMessageTracker _tracker;

    public CapEventSubscriber(
        ICustomerAppService customerSrv,
        ILogger<CapEventSubscriber> logger,
        MessageTrackerFactory trackerFactory)
    {
        _customerSrv = customerSrv;
        _logger = logger;
        _tracker = trackerFactory.Create();
    }

    /// <summary>
    /// 订阅充值事件
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(CustomerRechargedEvent))]
    public async Task ProcessCustomerRechargedEvent(CustomerRechargedEvent eventDto)
    {
        eventDto.EventTarget = nameof(ProcessCustomerRechargedEvent);
        var hasProcessed = await _tracker.HasProcessedAsync(eventDto);
        if (!hasProcessed)
            await _customerSrv.ProcessRechargingAsync(eventDto, _tracker);
    }

    /// <summary>
    /// 订阅付款事件
    /// </summary>
    /// <param name="eventDto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderPaidEvent))]
    public async Task ProcessOrderPaidEvent(OrderPaidEvent eventDto)
    {
        eventDto.EventTarget = nameof(ProcessOrderPaidEvent);
        var hasProcessed = await _tracker.HasProcessedAsync(eventDto);
        if (!hasProcessed)
            await Task.CompletedTask;
    }
}