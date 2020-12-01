using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{
    public interface ILogAppService : IAppService
    {

        Task<AppSrvResult<PageModelDto<LoginLogDto>>> GetLoginLogsPaged(LogSearchDto searchDto);

        Task<AppSrvResult<PageModelDto<OpsLogDto>>> GetOpsLogsPaged(LogSearchDto searchDto);

        Task<AppSrvResult<PageModelDto<NlogLogDto>>> GetNlogLogsPaged(LogSearchDto searchDto);
    }
}
