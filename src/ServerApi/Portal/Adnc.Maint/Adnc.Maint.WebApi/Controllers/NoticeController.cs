﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.WebApi.Shared;

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
        {
            _noticeService = noticeService;
        }

        /// <summary>
        /// 获取通知消息列表
        /// </summary>
        /// <param name="search"><see cref="NoticeSearchDto"/></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<List<NoticeDto>>> GetList([FromQuery] NoticeSearchDto search)
        {
            return Result(await _noticeService.GetListAsync(search));
        }
    }
}