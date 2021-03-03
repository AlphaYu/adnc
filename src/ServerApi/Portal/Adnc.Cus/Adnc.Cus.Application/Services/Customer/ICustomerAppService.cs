using System.Threading.Tasks;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Interceptors;
using Adnc.Cus.Application.Dtos;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Services
{
    public interface ICustomerAppService : IAppService
    {
        [OpsLog(LogName = "注册")]
        Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto inputDto);

        [OpsLog(LogName = "充值")]
        Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto inputDto);

        Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search);
    }
}
