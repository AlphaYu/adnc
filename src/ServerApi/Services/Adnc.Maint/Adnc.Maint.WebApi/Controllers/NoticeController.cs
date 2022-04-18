using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 通知管理
    /// </summary>
    [Route("maint/notices")]
    [ApiController]
    public class NoticeController : AdncControllerBase
    {
        private readonly INoticeAppService _noticeService;

        public NoticeController(INoticeAppService noticeService)
            => _noticeService = noticeService;

        /// <summary>
        /// 获取通知消息列表
        /// </summary>
        /// <param name="search"><see cref="NoticeSearchDto"/></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<List<NoticeDto>>> GetList([FromQuery] NoticeSearchDto search)
            => Result(await _noticeService.GetListAsync(search));
    }
}