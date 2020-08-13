using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    /// <summary>
    /// users 只缓存30秒，其他写操作无需修改缓存。
    /// </summary>
    public interface IUserAppService : IAppService
    {
        Task<PageModelDto<UserDto>> GetPaged(UserSearchDto searchModel);

        Task Save(UserSaveInputDto saveDto);

        Task Delete(long Id);

        Task SetRole(RoleSetInputDto setDto);

        Task ChangeStatus(long Id);

        Task ChangeStatus(UserChangeStatusInputDto changeDto);
    }
}