using System.Threading.Tasks;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;

namespace  Adnc.Maint.Application.Services
{
    public interface ICfgAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<CfgDto>>> GetPaged(CfgSearchDto searchDto);

        [OpsLog(LogName = "新增参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult<long>> Add(CfgSaveInputDto saveInputDto);

        [OpsLog(LogName = "修改参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> Update(CfgSaveInputDto saveInputDto);

        [OpsLog(LogName = "删除参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> Delete(long Id);

        Task<AppSrvResult<CfgDto>> Get(long Id);
    }
}
