using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Consts;

namespace Adnc.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 部门服务
    /// </summary>
    public interface IDeptAppService : IAppService
    {
        /// <summary>
        /// 部门树结构
        /// </summary>
        /// <returns></returns>
        [CachingAble(CacheKey = EasyCachingConsts.DetpTreeListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<AppSrvResult<List<DeptTreeDto>>> GetTreeListAsync();

        /// <summary>
        /// 精简的部门树结构
        /// </summary>
        /// <returns></returns>
        [CachingAble(CacheKey = EasyCachingConsts.DetpSimpleTreeListCacheKey, Expiration = EasyCachingConsts.OneYear)]
        Task<List<DeptSimpleTreeDto>> GetSimpleTreeListAsync();

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "新增部门")]
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.DeptCacheKeyPrefix, IsAll = true)]
        Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "修改部门")]
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.DeptCacheKeyPrefix, IsAll = true)]
        Task<AppSrvResult> UpdateAsync([CachingParam] long id, DeptUpdationDto input);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OpsLog(LogName = "删除部门")]
        [CachingEvict(CacheKeyPrefix = EasyCachingConsts.DeptCacheKeyPrefix, IsAll = true)]
        Task<AppSrvResult> DeleteAsync([CachingParam] long Id);
    }
}
