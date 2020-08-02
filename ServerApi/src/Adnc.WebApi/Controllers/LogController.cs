using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Application.Services;
using Adnc.Application.Dtos;
using Adnc.Core.Entities;

namespace Adnc.WebApi.Controllers
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Route("sys")]
    public class LogController : Controller
    {
        private readonly ILogAppService _logService;

        public LogController(ILogAppService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// 查询操作日志
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns></returns>
        [HttpGet("opslogs")]
        [Permission("opsLog")]
        public async Task<PageModelDto<OpsLogDto>> GetOpsLogsPaged([FromQuery] LogSearchDto searchDto)
        {
            return await _logService.GetOpsLogsPaged(searchDto);
        }

        /// <summary>
        /// 查询登录日志
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns></returns>
        [HttpGet("loginlogs")]
        [Permission("loginLog")]
        public async Task<PageModelDto<LoginLogDto>> GetLoginLogsPaged([FromQuery] LogSearchDto searchDto)
        {
            return await _logService.GetLoginLogsPaged(searchDto);
        }

        /// <summary>
        /// 查询Nlog日志
        /// </summary>
        /// <param name="searchDto">查询条件</param>
        /// <returns></returns>
        [HttpGet("nloglogs")]
        [Permission("nlogLog")]
        public async Task<PageModelDto<NlogLogDto>> GetNlogLogsPaged([FromQuery] LogSearchDto searchDto)
        {
            return await _logService.GetNlogLogsPaged(searchDto);
        }
    }
}
