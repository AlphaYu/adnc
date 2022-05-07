using Adnc.Shared.Rpc.Grpc;
using Adnc.Shared.Rpc.Grpc.Rtos;
using Adnc.Shared.Rpc.Grpc.Services;
using Adnc.Shared.Rpc.Rest.Services;

namespace Adnc.Maint.WebApi.Controllers;

/// <summary>
/// 通知管理
/// </summary>
[Route("maint/notices")]
[ApiController]
public class NoticeController : AdncControllerBase
{
    private readonly INoticeAppService _noticeService;
    private readonly IUsrRestClient _usrRestClient;
    private readonly UsrGrpc.UsrGrpcClient _usrGrpcClient;

    public NoticeController(INoticeAppService noticeService
        , IUsrRestClient usrRestService
        , UsrGrpc.UsrGrpcClient usrGrpcClient)
    {
        _noticeService = noticeService;
        _usrRestClient = usrRestService;
        _usrGrpcClient = usrGrpcClient;
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
    /// 测试Basic认证，获取用户服务部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("depts")]
    public async Task<IActionResult> GetDeptListAsync()
    {
        var depts = await _usrRestClient.GeDeptsAsync();
        if (depts.IsSuccessStatusCode && depts.Content.IsNotNullOrEmpty())
            return Ok(depts.Content);
        return NoContent();
    }


    /// <summary>
    ///  测试Basic(Grpc)认证，获取用户服务部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("deptsgrpc")]
    public async Task<IActionResult> GetDeptListGrpcAsync()
    {
        var grpcResult = await _usrGrpcClient.GetDeptsAsync(GrpcClientConsts.Empty, GrpcClientConsts.BasicHeader);
        if (grpcResult.IsSuccessStatusCode && grpcResult.Content.Is(DeptListReply.Descriptor))
        {
            var outputDto = grpcResult.Content.Unpack<DeptListReply>();
            return Ok(outputDto);
        }
        return BadRequest(grpcResult);
    }
}