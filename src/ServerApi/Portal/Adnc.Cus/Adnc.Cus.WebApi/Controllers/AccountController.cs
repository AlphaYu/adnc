using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Shared.RpcServices;
using System.Text.Json;
using Refit;

namespace Adnc.Cus.WebApi.Controllers
{
    [Route("cus/session")]
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
        public async Task<LoginReply> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _authRpcService.Login(loginRequest);
            return result;
            //var result = await RestService.For<IAuthRpcService>("http://localhost:5010").Login(loginRequest);
            //return result;
        }
    }
}
