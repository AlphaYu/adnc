using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Application.Interceptors.OpsLog;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    /// <summary>
    /// users 只缓存30秒，其他写操作无需修改缓存。
    /// </summary>
    public interface IUserAppService : IAppService
    {
        Task<PageModelDto<UserDto>> GetPaged(UserSearchDto searchModel);

        [OpsLog(LogName = "新增/修改用户")]
        Task Save(UserSaveInputDto saveDto);

        [OpsLog(LogName = "删除用户")]
        Task Delete(long Id);

        [OpsLog(LogName = "设置用户角色")]
        Task SetRole(RoleSetInputDto setDto);

        [OpsLog(LogName = "修改用户状态")]
        Task ChangeStatus(long Id);

        [OpsLog(LogName = "批量修改用户状态")]
        Task ChangeStatus(UserChangeStatusInputDto changeDto);
    }
}