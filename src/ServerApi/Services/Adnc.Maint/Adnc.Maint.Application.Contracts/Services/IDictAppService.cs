using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Maint.Application.Contracts.Consts;

namespace Adnc.Maint.Application.Contracts.Services
{
    public interface IDictAppService : IAppService
    {
        Task<AppSrvResult<List<DictDto>>> GetListAsync(DictSearchDto serach);

        Task<AppSrvResult<DictDto>> GetAsync(long id);

        [OpsLog(LogName = "新增字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(DictCreationDto input);

        [OpsLog(LogName = "修改字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, DictUpdationDto input);

        [OpsLog(LogName = "删除字典")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.DictListCacheKey)]
        Task<AppSrvResult> DeleteAsync(long id);
    }
}
