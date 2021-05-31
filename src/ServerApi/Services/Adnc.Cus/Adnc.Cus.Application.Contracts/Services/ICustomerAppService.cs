using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.Interceptors;
using Adnc.Cus.Application.Contracts.Dtos;
using System.Threading.Tasks;

namespace Adnc.Cus.Application.Contracts.Services
{
    public interface ICustomerAppService : IAppService
    {
        [OpsLog(LogName = "注册")]
        Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto inputDto);

        [OpsLog(LogName = "充值")]
        [UnitOfWork(SharedToCap = true)]
        Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto inputDto);

        [OpsLog(LogName = "处理充值")]
        [UnitOfWork]
        Task<AppSrvResult> ProcessRechargingAsync(long transactionLogId, long customerId, decimal amount);

        [OpsLog(LogName = "处理付款")]
        [UnitOfWork(SharedToCap = true)]
        Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount);

        Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search);
    }
}