using System.Threading.Tasks;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.Application.Contracts.Services
{
    public interface ILogAppService : IAppService
    {
        Task<AppSrvResult<PageModelDto<LoginLogDto>>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto);

        Task<AppSrvResult<PageModelDto<OpsLogDto>>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto);

        Task<AppSrvResult<PageModelDto<NlogLogDto>>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto);
    }
}
