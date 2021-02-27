using System.Threading.Tasks;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Ord.Application.Dtos;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Ord.Application.Services
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public interface IOrderAppService : IAppService
    {
        [OpsLog(LogName = "订单创建")]
        [UnitOfWork(SharedToCap = true)]
        Task<OrderDto> CreateAsync(OrderCreationDto input);

        [OpsLog(LogName = "订单付款")]
        [UnitOfWork(SharedToCap = true)]
        Task<OrderDto> PayAsync(long id);

        [OpsLog(LogName = "订单更新")]
        Task<OrderDto> UpdateAsync(long id, OrderUpdationDto input);

        [OpsLog(LogName = "订单取消")]
        [UnitOfWork(SharedToCap = true)]
        Task<OrderDto> CancelAsync(long id);

        [OpsLog(LogName = "订单删除")]
        Task DeleteAsync(long id);

        [OpsLog(LogName = "订单搜索")]
        Task<PageModelDto<OrderDto>> GetPagedAsync(OrderSearchPagedDto search);

        [OpsLog(LogName = "订单详情")]
        Task<OrderDto> GetAsync(long id);
    }
}
