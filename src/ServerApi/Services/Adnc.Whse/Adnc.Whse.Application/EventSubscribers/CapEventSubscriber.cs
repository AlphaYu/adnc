namespace Adnc.Whse.Application.EventSubscribers;

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
    /// 订阅订单创建事件
    /// </summary>
    /// <param name="warehouseQtyBlockedEvent"></param>
    /// <returns></returns>
    [CapSubscribe(nameof(OrderCreatedEvent))]
    public async Task ProcessOrderCreatedEvent(OrderCreatedEvent eto)
    {
        using var scope = _services.CreateScope();
        var data = eto.Data;
        var appSrv = scope.ServiceProvider.GetRequiredService<IWarehouseAppService>();
        await appSrv.BlockQtyAsync(new WarehouseBlockQtyDto { OrderId = data.OrderId, Products = data.Products.Select(x => (x.ProductId, x.Qty)) });
    }

    #endregion across service event
}