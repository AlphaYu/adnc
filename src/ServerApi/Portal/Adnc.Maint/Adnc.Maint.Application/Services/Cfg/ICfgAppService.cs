using System.Threading.Tasks;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Services
{
    public interface ICfgAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<CfgDto>>> GetPagedAsync(CfgSearchPagedDto search);

        [OpsLog(LogName = "新增参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult<long>> CreateAsync(CfgCreationDto input);

        [OpsLog(LogName = "修改参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> UpdateAsync(long id, CfgUpdationDto input);

        [OpsLog(LogName = "删除参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task<AppSrvResult> DeleteAsync(long id);

        Task<AppSrvResult<CfgDto>> GetAsync(long id);
    }
}
