namespace Adnc.Demo.Ord.Application.Services;

/// <summary>
/// 订单管理
/// </summary>
public interface IOrderAppService : IAppService
{
    [OperateLog(LogName = "订单创建")]
    [UnitOfWork(SharedToCap = true)]
    Task<OrderDto> CreateAsync(OrderCreationDto input);

    [OperateLog(LogName = "调整订单状态")]
    [UnitOfWork]
    Task MarkCreatedStatusAsync(WarehouseQtyBlockedEvent eventDto, IMessageTracker tracker);

    [OperateLog(LogName = "订单付款")]
    [UnitOfWork(SharedToCap = true)]
    Task<OrderDto> PayAsync(long id);

    [OperateLog(LogName = "订单更新")]
    Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input);

    [OperateLog(LogName = "订单取消")]
    [UnitOfWork(SharedToCap = true)]
    Task<OrderDto> CancelAsync(long id);

    [OperateLog(LogName = "订单删除")]
    Task DeleteAsync(long id);

    [OperateLog(LogName = "订单搜索")]
    Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto search);

    [OperateLog(LogName = "订单详情")]
    Task<OrderDto> GetAsync(long id);
}