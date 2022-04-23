namespace Adnc.Ord.WebApi.Controllers;

[Route("ord/session")]
[ApiController]
public class AccountController : AdncControllerBase
{
    private readonly IAuthRpcService _authRpcService;

    public AccountController(IAuthRpcService authRpcService) => _authRpcService = authRpcService;

    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] LoginRto input)
    {
        var result = await _authRpcService.LoginAsync(input);

        if (result.IsSuccessStatusCode)
            return Ok(result.Content);

        return Problem(result.Error);
    }
}