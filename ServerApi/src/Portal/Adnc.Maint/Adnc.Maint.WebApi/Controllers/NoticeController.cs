using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 通知
    /// </summary>
    [Route("maint/notices")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeAppService _noticeService;

        public NoticeController(INoticeAppService noticeService)
        {
            _noticeService = noticeService;
        }

        /// <summary>
        /// 获取通知消息列表
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<List<NoticeDto>> GetList([FromQuery]string title)
        {
            return await _noticeService.GetList(title);
        }
    }
}