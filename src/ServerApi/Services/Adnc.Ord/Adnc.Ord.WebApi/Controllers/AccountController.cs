namespace Adnc.Ord.WebApi.Controllers;

[Route("ord/session")]
[ApiController]
public class AccountController : AdncControllerBase
{
    private readonly IAuthRestClient _authRestClient;

    public AccountController(IAuthRestClient authRestClient) => _authRestClient = authRestClient;

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
}