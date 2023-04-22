namespace Adnc.Demo.Cust.Api.Controllers;

/// <summary>
/// REST and gRPC demo
/// </summary>
[Route($"{RouteConsts.CustRoot}/restandgrpcdemo")]
[ApiController]
public class RestAndGrpcClientDemoController : AdncControllerBase
{
    private readonly IAuthRestClient _authRestClient;
    private readonly IUsrRestClient _usrRestClient;
    private readonly IMaintRestClient _maintRestClient;
    private readonly IWhseRestClient _whseRestClient;
    private readonly AuthGrpc.AuthGrpcClient _authGrpcClinet;
    private readonly UsrGrpc.UsrGrpcClient _usrGrpcClient;
    private readonly MaintGrpc.MaintGrpcClient _maintGrpcClient;
    private readonly WhseGrpc.WhseGrpcClient _whseGrpcClient;

    public RestAndGrpcClientDemoController(
    IAuthRestClient authRestClient,
     IUsrRestClient usrRestClient,
     IMaintRestClient maintRestClient,
     IWhseRestClient whseRestClient,
     AuthGrpc.AuthGrpcClient authGrpcClinet,
     UsrGrpc.UsrGrpcClient usrGrpcClient,
     MaintGrpc.MaintGrpcClient maintGrpcClient,
     WhseGrpc.WhseGrpcClient whseGrpcClient)
    {
        _authRestClient = authRestClient;
        _usrRestClient = usrRestClient;
        _maintRestClient = maintRestClient;
        _whseRestClient = whseRestClient;
        _authGrpcClinet = authGrpcClinet;
        _usrGrpcClient = usrGrpcClient;
        _maintGrpcClient = maintGrpcClient;
        _whseGrpcClient = whseGrpcClient;
    }

    /// <summary>
    /// 测试后台管理员登录-REST
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("restsession")]
    public async Task<IActionResult> Login([FromBody] LoginInputRto input)
    {
        var result = await _authRestClient.LoginAsync(input);

        if (result.IsSuccessStatusCode)
            return Ok(result.Content);

        return Problem(result.Error);
    }

    /// <summary>
    /// 测试后台管理员登录-GRPC
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("grpcsession")]
    public async Task<IActionResult> LoginGrpcAsync([FromBody] LoginRequest input)
    {
        var result = await _authGrpcClinet.LoginAsync(input);

        if (result.IsSuccessStatusCode && result.Content.Is(LoginReply.Descriptor))
        {
            var outputDto = result.Content.Unpack<LoginReply>();
            return Ok(outputDto);
        }
        return BadRequest(result);
    }

    /// <summary>
    /// 测试Basic(Rest)认证，获取用户服务部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("restdepts")]
    public async Task<IActionResult> GetDeptListAsync()
    {
        var restResult = await _usrRestClient.GetOrganizationsAsync();
        if (restResult.IsSuccessStatusCode && restResult.Content.IsNotNullOrEmpty())
            return Ok(restResult.Content);
        return NoContent();
    }

    /// <summary>
    ///  测试Basic(Grpc)认证，获取用户服务部门列表
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("grpcdepts")]
    public async Task<IActionResult> GetDeptListGrpcAsync()
    {
        var grpcResult = await _usrGrpcClient.GetDeptsAsync(GrpcClientConsts.Empty, GrpcClientConsts.BasicHeader);
        if (grpcResult.IsSuccessStatusCode && grpcResult.Content.Is(DeptListReply.Descriptor))
        {
            var unpackResult = grpcResult.Content.Unpack<DeptListReply>();
            return Ok(unpackResult);
        }
        return NoContent();
    }

    /// <summary>
    /// 测试Basic(Rest)认证，获取字典
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("restdict")]
    public async Task<IActionResult> GetDictAsync()
    {
        var restResult = await _maintRestClient.GetDictAsync(ServiceAddressConsts.OrderStatusId);
        if (restResult.IsSuccessStatusCode && restResult.Content is not null)
            return Ok(restResult.Content);
        return NoContent();
    }

    /// <summary>
    ///  测试Basic(Grpc)认证，获取字典
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("grpcdict")]
    public async Task<IActionResult> GetDictGrpcAsync()
    {
        var request = new DictRequest() { Id = ServiceAddressConsts.OrderStatusId };
        var grpcResult = await _maintGrpcClient.GetDictAsync(request, GrpcClientConsts.BasicHeader);
        if (grpcResult.IsSuccessStatusCode && grpcResult.Content.Is(DictReply.Descriptor))
        {
            var unpackResult = grpcResult.Content.Unpack<DictReply>();
            return Ok(unpackResult);
        }
        return NoContent();
    }

    /// <summary>
    /// 测试Basic认证(Rest)，获取产品
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("restproductions")]
    public async Task<IActionResult> GetProductionsAsync()
    {
        var searchDto = new ProductSearchListRto()
        {
            Ids = new long[] { 285806185760389, 285806311700101 },
            StatusCode = 1000
        };
        var restResult = await _whseRestClient.GetProductsAsync(searchDto);
        if (restResult.IsSuccessStatusCode && restResult.Content is not null)
            return Ok(restResult.Content);
        return NoContent();
    }

    /// <summary>
    ///  测试Basic(Grpc)认证，获取产品
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Route("grpcproductions")]
    public async Task<IActionResult> GetProductionsGrpcAsync()
    {
        var request = new ProductSearchRequest
        {
            StatusCode = 1000
        };
        request.Ids.AddRange(new long[] { 285806185760389, 285806311700101 });
        var grpcResult = await _whseGrpcClient.GetProductsAsync(request, GrpcClientConsts.BasicHeader);
        if (grpcResult.IsSuccessStatusCode && grpcResult.Content.Is(ProductListReply.Descriptor))
        {
            var unpackResult = grpcResult.Content.Unpack<ProductListReply>();
            return Ok(unpackResult);
        }
        return NoContent();
    }
}