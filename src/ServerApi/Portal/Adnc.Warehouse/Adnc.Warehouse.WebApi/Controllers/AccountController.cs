using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Shared.RpcServices;
using Adnc.WebApi.Shared;

namespace Adnc.Cus.WebApi.Controllers
{
    [Route("cus/session")]
    [ApiController]
    public class AccountController : AdncControllerBase
    {
        private readonly IAuthRpcService _authRpcService;

        public AccountController(IAuthRpcService authRpcService)
        {
            _authRpcService = authRpcService;
        }

        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authRpcService.Login(loginRequest);

            if (result.IsSuccessStatusCode)
                return Ok(result.Content);

            var apiError = ((Refit.ValidationApiException)result.Error).Content;
            return Problem(apiError.Detail, result.Error.Uri.ToString(), apiError.Status, apiError.Title, apiError.Type);
        }
    }
}
