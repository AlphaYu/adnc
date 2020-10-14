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
        Task Register(RegisterInputDto inputDto);

        [OpsLog(LogName = "充值")]
        Task<SimpleDto<string>> Recharge(RechargeInputDto inputDto);
    }
}
