namespace Adnc.Maint.WebApi.Controllers;

/// <summary>
/// 通知管理
/// </summary>
[Route("maint/notices")]
[ApiController]
public class NoticeController : AdncControllerBase
{
    private readonly INoticeAppService _noticeService;
    private readonly UserContext _userContext;

    public NoticeController(
        INoticeAppService noticeService,
        UserContext userContext)
    {
        _noticeService = noticeService;
        _userContext = userContext;
    }

    /// <summary>
    /// 获取通知消息列表
    /// </summary>
    /// <param name="search"><see cref="NoticeSearchDto"/></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<List<NoticeDto>>> GetList([FromQuery] NoticeSearchDto search)
    {
        if (_userContext is null || _userContext.Id == 0)
            return await Task.FromResult(new List<NoticeDto>());
        else
            return Result(await _noticeService.GetListAsync(search));
    }
}