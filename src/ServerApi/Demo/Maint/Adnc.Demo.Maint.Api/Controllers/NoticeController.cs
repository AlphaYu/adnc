namespace Adnc.Demo.Maint.WebApi.Controllers;

/// <summary>
/// 通知管理
/// </summary>
[Route($"{RouteConsts.MaintRoot}/notices")]
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
            return Result(await _noticeService.GetListAsync(search));
    }
}