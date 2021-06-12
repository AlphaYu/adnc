using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Maint.Application.Contracts.Dtos;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Contracts.Services
{
    /// <summary>
    /// 日志查询
    /// </summary>
    public interface ILogAppService : IAppService
    {
        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(LogSearchPagedDto searchDto);

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        Task<PageModelDto<OpsLogDto>> GetOpsLogsPagedAsync(LogSearchPagedDto searchDto);

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(LogSearchPagedDto searchDto);
    }
}