using System.Threading;
using System.Threading.Tasks;
using Adnc.Application.Dtos;

namespace Adnc.Application.Services
{
    public interface ILogAppService : IAppService
    {

        Task<PageModelDto<LoginLogDto>> GetLoginLogsPaged(LogSearchDto searchDto);

        //[EasyCachingAble(CacheKeyPrefix = EasyCachingConsts.SearchOperationLogsKeyPrefix, Expiration = 10)]
        Task<PageModelDto<OpsLogDto>> GetOpsLogsPaged(LogSearchDto searchDto);

        Task<PageModelDto<NlogLogDto>> GetNlogLogsPaged(LogSearchDto searchDto);
    }
}
