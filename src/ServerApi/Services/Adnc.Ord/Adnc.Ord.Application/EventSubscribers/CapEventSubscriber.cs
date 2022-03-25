namespace Adnc.Ord.Application.EventSubscribers;

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

    #region across service event

    /// <summary>
    /// 订阅库存锁定事件
    /// </summary>
    /// <param name="warehouseQtyBlockedEvent"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(WarehouseQtyBlockedEvent))]
    public async Task ProcessWarehouseQtyBlockedEvent(WarehouseQtyBlockedEvent eto)

    {
        var data = eto.Data;
        using var scope = _services.CreateScope();
        var appSrv = scope.ServiceProvider.GetRequiredService<IOrderAppService>();
        await appSrv.MarkCreatedStatusAsync(data.OrderId, new OrderMarkCreatedStatusDto { IsSuccess = data.IsSuccess, Remark = data.Remark });
    }

    #endregion across service event
}