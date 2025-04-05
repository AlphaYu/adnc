using Adnc.Demo.Remote.Grpc;
using Adnc.Demo.Remote.Grpc.Messages;
using Adnc.Demo.Remote.Grpc.Services;

namespace Adnc.Demo.Cust.Api.Controllers;

/// <summary>
/// gRPC demo
/// </summary>
[Route($"{RouteConsts.CustRoot}/grpc")]
[ApiController]
public class GrpcClientDemoController(AdminGrpc.AdminGrpcClient adminGrpcClient) : AdncControllerBase
{
    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    [AllowAnonymous, HttpGet()]
    [Route("dictoptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DictOptionReply>>> GetDictOptionsAsync(string codes = "all")
    {
        var request = new DictOptionRequest { Codes = codes };
        var grpcResult = await adminGrpcClient.GetDictOptionsAsync(request, GrpcClientConsts.BasicHeader);
        return grpcResult is not null ? grpcResult.List.ToList() : [];
    }

    /// <summary>
    /// 获取系统配置列表
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    [AllowAnonymous, HttpGet()]
    [Route("sysconfigs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SysConfigSimpleReply>>> GetSysConfigListAsync(string keys = "all")
    {
        var request = new SysConfigSimpleRequest { Keys = keys };
        var grpcResult = await adminGrpcClient.GetSysConfigListAsync(request, GrpcClientConsts.BasicHeader);
        return grpcResult is not null ? grpcResult.List.ToList() : [];
    }
}
