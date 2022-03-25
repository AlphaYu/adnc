namespace Adnc.Cus.Application.EventSubscribers;

public sealed class CapEventSubscriber : ICapSubscribe
{
    private readonly IServiceProvider _services;
    private readonly ILogger<CapEventSubscriber> _logger;

    public CapEventSubscriber(
        IServiceProvider services
        , ILogger<CapEventSubscriber> logger)
    {
        _services = services;
        _logger = logger;
    }

    #region local event

    /// <summary>
    /// 订阅充值事件
    /// </summary>
    /// <param name="eto"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(CustomerRechargedEvent))]
    public async Task ProcessCustomerRechargedEvent(CustomerRechargedEvent eto)
    {
        using var scope = _services.CreateScope();
        var appSrv = scope.ServiceProvider.GetRequiredService<ICustomerAppService>();
        await appSrv.ProcessRechargingAsync(eto.Data.TransactionLogId, eto.Data.CustomerId, eto.Data.Amount);
    }

    #endregion local event

    #region across service event

    /// <summary>
    /// 订阅付款事件
    /// </summary>
    /// <param name="warehouseQtyBlockedEvent"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderPaidEvent))]
    public async Task ProcessOrderPaidEvent(OrderPaidEvent eto)
    {
        _logger.LogInformation("start.....");
        _logger.LogInformation("end.....");
        await Task.CompletedTask;
    }

    #endregion across service event
}