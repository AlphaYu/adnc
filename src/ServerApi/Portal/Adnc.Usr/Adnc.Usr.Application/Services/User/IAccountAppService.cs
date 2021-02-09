using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public interface IAccountAppService : IAppService
    {
        Task<AppSrvResult<UserValidateDto>> LoginAsync(UserLoginDto input);

        Task<AppSrvResult<UserInfoDto>> GetUserInfoAsync(long userId);

        [OpsLog(LogName = "修改密码")]
        Task<AppSrvResult> UpdatePasswordAsync(long id, UserChangePwdDto input);

        Task<AppSrvResult<UserValidateDto>> GetUserValidateInfoAsync(string account);
    }
}
