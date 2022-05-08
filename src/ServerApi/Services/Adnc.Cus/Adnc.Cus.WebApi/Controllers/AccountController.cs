using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.Rpc.Grpc.Services;

namespace Adnc.Cus.WebApi.Controllers;

[Route("cus/session")]
[ApiController]
public class AccountController : AdncControllerBase
{
    private readonly IAuthRestClient _authRestClient;
    private readonly AuthGrpc.AuthGrpcClient _authGrpcClinet;

    public AccountController(IAuthRestClient authRestClient, AuthGrpc.AuthGrpcClient authGrpcClient)
    {
        _authRestClient = authRestClient;
        _authGrpcClinet = authGrpcClient;
    }

    /// <summary>
    /// for debugging purposes
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] LoginInputRto input)
    {
        var result = await _authRestClient.LoginAsync(input);

        if (result.IsSuccessStatusCode)
            return Ok(result.Content);

        return Problem(result.Error);
    }

    /// <summary>
    /// for debugging purposes(grpc)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("grpc")]
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
}