using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Consts;
using Adnc.Usr.Application.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Usr.Application.Contracts.Services
{
    /// <summary>
    /// 部门服务
    /// </summary>
    public interface IDeptAppService : IAppService
    {
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "新增部门")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input);

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "修改部门")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OpsLog(LogName = "删除部门")]
        [CachingEvict(CacheKeys = new string[] { CachingConsts.DetpListCacheKey, CachingConsts.DetpTreeListCacheKey, CachingConsts.DetpSimpleTreeListCacheKey })]
        Task<AppSrvResult> DeleteAsync(long Id);

        /// <summary>
        /// 部门树结构
        /// </summary>
        /// <returns></returns>
        [CachingAble(CacheKey = CachingConsts.DetpTreeListCacheKey, Expiration = CachingConsts.OneYear)]
        Task<List<DeptTreeDto>> GetTreeListAsync();
    }
}