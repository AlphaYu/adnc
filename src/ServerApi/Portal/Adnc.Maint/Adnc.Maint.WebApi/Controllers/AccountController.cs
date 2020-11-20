using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Shared.RpcServices;

namespace Adnc.Maint.WebApi.Controllers
{
    [Route("maint/session")]
    [ApiController]
    public class AccountController : ControllerBase
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
                return new OkObjectResult(result.Content);

            return new JsonResult(result.Error.Content)
            {
                StatusCode = (int)result.StatusCode
            };
        }
    }
}
