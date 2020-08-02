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
        [EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, Expiration = 60)]
        Task<PageModelDto<UserDto>> GetPaged(UserSearchDto searchModel);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, IsAll = true)]
        Task<int> Save(UserSaveInputDto saveDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, IsAll = true)]
        Task<int> Delete(long Id);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, IsAll = true)]
        Task<int> SetRole(RoleSetInputDto setDto);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, IsAll = true)]
        Task<int> ChangeStatus(long Id);

        [EasyCachingEvict(CacheKeyPrefix = EasyCachingConsts.SearchUsersKeyPrefix, IsAll = true)]
        Task<int> ChangeStatus(UserChangeStatusInputDto changeDto);
    }
}