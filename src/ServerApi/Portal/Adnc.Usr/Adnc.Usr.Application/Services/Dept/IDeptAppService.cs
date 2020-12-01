using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Usr.Application.Services
{
    public interface IDeptAppService : IAppService
    {
        Task<AppSrvResult<List<DeptNodeDto>>> GetList();

        [OpsLog(LogName = "新增部门")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult<long>> Add(DeptSaveInputDto savetDto);

        [OpsLog(LogName = "修改部门")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult> Update(DeptSaveInputDto savetDto);

        [OpsLog(LogName = "删除部门")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task<AppSrvResult> Delete(long Id);

        Task<List<DeptDto>> GetAllFromCache();

        Task<dynamic[]> GetSimpleList();
    }
}
