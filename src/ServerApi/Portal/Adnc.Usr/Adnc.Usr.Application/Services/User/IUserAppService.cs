using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Usr.Application.Services
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public interface IUserAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<UserDto>>> GetPaged(UserSearchDto searchModel);

        [OpsLog(LogName = "新增用户")]
        Task<AppSrvResult<long>> Add(UserSaveInputDto saveDto);

        [OpsLog(LogName = "修改用户")]
        Task<AppSrvResult> Update(UserSaveInputDto saveDto);

        [OpsLog(LogName = "删除用户")]
        Task<AppSrvResult> Delete(long Id);

        [OpsLog(LogName = "设置用户角色")]
        Task<AppSrvResult> SetRole(RoleSetInputDto setDto);

        [OpsLog(LogName = "修改用户状态")]
        Task<AppSrvResult> ChangeStatus(long Id, int status);

        [OpsLog(LogName = "批量修改用户状态")]
        Task<AppSrvResult> ChangeStatus(UserChangeStatusInputDto changeDto);
    }
}