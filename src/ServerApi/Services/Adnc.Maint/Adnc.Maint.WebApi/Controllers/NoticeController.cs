namespace Adnc.Maint.WebApi.Controllers;

/// <summary>
/// 通知管理
/// </summary>
[Route("maint/notices")]
[ApiController]
public class NoticeController : AdncControllerBase
{
    private readonly INoticeAppService _noticeService;
    private readonly IUsrRpcService _usrRpcService;

    public NoticeController(INoticeAppService noticeService
        , IUsrRpcService usrRpcService)
    {
        _noticeService = noticeService;
        _usrRpcService = usrRpcService;
    }

    /// <summary>
    /// 获取通知消息列表
    /// </summary>
    /// <param name="search"><see cref="NoticeSearchDto"/></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<ActionResult<List<NoticeDto>>> GetList([FromQuery] NoticeSearchDto search)
        => Result(await _noticeService.GetListAsync(search));

    /// <summary>
    /// 测试Basic认证用途，获取用户服务部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("depts")]
    public async Task<IActionResult> GetDeptListAsync()
    {
        var depts = await _usrRpcService.GeDeptsAsync();
        if (depts.IsNotNullOrEmpty())
            return Ok(depts);
        return NoContent();
    }
}