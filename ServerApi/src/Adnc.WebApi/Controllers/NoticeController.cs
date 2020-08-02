using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Services;
using Adnc.Application.Dtos;

namespace Adnc.WebApi.Controllers
{
    /// <summary>
    /// 通知
    /// </summary>
    [Route("sys/notices")]
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