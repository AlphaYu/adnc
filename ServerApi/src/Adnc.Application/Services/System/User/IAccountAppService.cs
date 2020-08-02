using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;

namespace Adnc.Application.Services
{
    public interface IAccountAppService : IAppService
    {
        Task<UserValidateDto> Login(UserValidateInputDto userDto);

        Task<UserInfoDto> GetCurrentUserInfo();

        Task UpdatePassword(UserChangePwdInputDto passwordDto);

        Task<UserValidateDto> GetUserValidateInfo(RefreshTokenInputDto tokenInfo);
    }
}
