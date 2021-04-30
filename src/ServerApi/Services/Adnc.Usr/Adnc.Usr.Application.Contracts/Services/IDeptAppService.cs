using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching.Interceptor;
using Adnc.Usr.Application.Contracts.Consts;

namespace Adnc.Usr.Application.Contracts.Services
{
    public interface IDeptAppService : IAppService
    {
        Task<AppSrvResult<List<DeptTreeeDto>>> GetListAsync();

        [OpsLog(LogName = "新增部门")]
        [CachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(DeptCreationDto input);

        [OpsLog(LogName = "修改部门")]
        [CachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input);

        [OpsLog(LogName = "删除部门")]
        [CachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult> DeleteAsync(long Id);

        Task<dynamic[]> GetSimpleListAsync();
    }
}
