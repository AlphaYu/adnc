using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public interface IAccountAppService : IAppService
    {
        [OpsLog(LogName = "登录")]
        Task<AppSrvResult<UserValidateDto>> Login(UserValidateInputDto userDto);

        Task<AppSrvResult<UserInfoDto>> GetUserInfo(long userId);

        [OpsLog(LogName = "修改密码")]
        Task<AppSrvResult> UpdatePassword(UserChangePwdInputDto passwordDto, long userId);

        Task<AppSrvResult<UserValidateDto>> GetUserValidateInfo(RefreshTokenInputDto tokenInfo);
    }
}
