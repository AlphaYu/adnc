using System.Threading.Tasks;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Application.Dtos;
using Adnc.Application.Interceptors.OpsLog;

namespace Adnc.Application.Services
{
    public interface ICfgAppService : IAppService
    {
        Task<PageModelDto<CfgDto>> GetPaged(CfgSearchDto searchDto);

        [OpsLog(LogName = "新增/修改参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task Save(CfgSaveInputDto saveInputDto);

        [OpsLog(LogName = "删除参数")]
        [EasyCachingEvict(CacheKey = EasyCachingConsts.CfgListCacheKey)]
        Task Delete(long Id);

        Task<CfgDto> Get(long Id);
    }
}
