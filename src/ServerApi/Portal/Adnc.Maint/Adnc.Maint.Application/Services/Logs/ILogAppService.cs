using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{
    public interface ILogAppService : IAppService
    {

        Task<AppSrvResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto);

        Task<AppSrvResult<PageModelDto<OpsLogDto>>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto);

        Task<AppSrvResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto);
    }
}
