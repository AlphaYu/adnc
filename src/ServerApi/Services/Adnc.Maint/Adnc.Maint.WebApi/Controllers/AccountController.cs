using Adnc.Shared.Rpc.Rest.Rtos;
using Adnc.Shared.Rpc.Rest.Services;

namespace Adnc.Maint.WebApi.Controllers;

[Route("maint/session")]
[ApiController]
public class AccountController : AdncControllerBase
{
    private readonly IAuthRestClient _authRestClient;

    public AccountController(IAuthRestClient authRestClinet) => _authRestClient = authRestClinet;

    /// <summary>
    /// for debugging purposes(rest)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> LoginRestAsync([FromBody] LoginInputRto input)
    {
        var result = await _authRestClient.LoginAsync(input);

        if (result.IsSuccessStatusCode)
            return Ok(result.Content);

        return Problem(result.Error);
    }
}