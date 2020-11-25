using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;

namespace Adnc.Usr.Application.Services
{
    public interface IAccountAppService : IAppService
    {
        Task<UserValidateDto> Login(UserValidateInputDto userDto);

        Task<UserInfoDto> GetUserInfo(long userId);

        [OpsLog(LogName = "修改密码")]
        Task<UserValidateDto> UpdatePassword(UserChangePwdInputDto passwordDto, long userId);

        Task<UserValidateDto> GetUserValidateInfo(RefreshTokenInputDto tokenInfo);
    }
}
