using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;

namespace  Adnc.Maint.Application.Services
{
    public interface IDictAppService : IAppService
    {
        Task<AppSrvResult<List<DictDto>>> GetList(DictSearchDto searchDto);

        Task<AppSrvResult<DictDto>> Get(long id);

        [OpsLog(LogName = "新增字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult<long>> Add(DictSaveInputDto saveDto);

        [OpsLog(LogName = "修改字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult> Update(DictSaveInputDto saveDto);

        [OpsLog(LogName = "删除字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult> Delete(long Id);
    }
}
