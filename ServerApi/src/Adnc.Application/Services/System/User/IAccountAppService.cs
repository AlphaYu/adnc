using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Application.Interceptors.OpsLog;

namespace Adnc.Application.Services
{
    public interface IAccountAppService : IAppService
    {
        Task<UserValidateDto> Login(UserValidateInputDto userDto);

        Task<UserInfoDto> GetCurrentUserInfo();

        [OpsLog(LogName = "修改密码")]
        Task<UserValidateDto> UpdatePassword(UserChangePwdInputDto passwordDto);

        Task<UserValidateDto> GetUserValidateInfo(RefreshTokenInputDto tokenInfo);
    }
}
