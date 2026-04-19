using Adnc.Demo.Ord.Application.Contracts.Dtos.Order;
using Adnc.Demo.Remote.Event;
using Adnc.Infra.EventBus.Tracker;

namespace Adnc.Demo.Ord.Application.Contracts.Interfaces;

/// <summary>
/// Order management
/// </summary>
public interface IOrderService : IAppService
{
    [OperateLog(LogName = "Create order")]
    [UnitOfWork(Distributed = true)]
    Task<OrderDto> CreateAsync(OrderCreationDto input);

    [OperateLog(LogName = "Change order status")]
    [UnitOfWork]
    Task MarkCreatedStatusAsync(Remote.Event.WarehouseQtyBlockedEvent eventDto, IMessageTracker tracker);

    [OperateLog(LogName = "Pay order")]
    [UnitOfWork(Distributed = true)]
    Task<OrderDto> PayAsync(long id);

    [OperateLog(LogName = "Update order")]
    Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input);

    [OperateLog(LogName = "Cancel order")]
    [UnitOfWork(Distributed = true)]
    Task<OrderDto> CancelAsync(long id);

    [OperateLog(LogName = "Delete order")]
    Task DeleteAsync(long id);

    [OperateLog(LogName = "Search orders")]
    Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto input);

    [OperateLog(LogName = "Order details")]
    Task<OrderDto> GetAsync(long id);
}
