using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Application.Interceptors.OpsLog;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace Adnc.Application.Services
{
    public interface IDeptAppService : IAppService
    {
        Task<List<DeptNodeDto>> GetList();

        [OpsLog(LogName = "新增/修改部门")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task Save(DeptSaveInputDto savetDto);

        [OpsLog(LogName = "删除部门")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DetpListCacheKey)]
        Task Delete(long Id);
    }
}
