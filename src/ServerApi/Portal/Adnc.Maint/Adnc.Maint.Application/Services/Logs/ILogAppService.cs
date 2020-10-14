using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{
    public interface ILogAppService : IAppService
    {

        Task<PageModelDto<LoginLogDto>> GetLoginLogsPaged(LogSearchDto searchDto);

        //[EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.SearchOperationLogsKeyPrefix, Expiration = 10)]
        Task<PageModelDto<OpsLogDto>> GetOpsLogsPaged(LogSearchDto searchDto);

        Task<PageModelDto<NlogLogDto>> GetNlogLogsPaged(LogSearchDto searchDto);
    }
}
