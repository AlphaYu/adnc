using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Adnc.WebApi.Helper;
using Microsoft.Extensions.Logging;
using Adnc.Application.Dtos;
using Adnc.Common.Models;
using Adnc.Application.Services;
using Adnc.Infr.Consul.Consumer;

namespace Adnc.WebApi.Controllers
{
    /// <summary>
    /// 验证/授权/注销
    /// </summary>
    [Route("sys/session")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IAccountAppService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IOptions<JWTConfig> jwtConfig
            , IAccountAppService accountService
            , ILogger<AccountController> logger)
        {
            _jwtConfig = jwtConfig.Value;
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// 登录/验证
        /// </summary>
        /// <param name="userDto"><see cref="UserValidateInputDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody]UserValidateInputDto userDto)
        {
            var address = await ServiceConsumer.GetServicesAsync("http://localhost:8510", "andc-api-sys");

            var userValidateDto = await _accountService.Login(userDto);

            return new OkObjectResult(new
            {
                Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, userValidateDto),
                RefreshToken = JwtTokenHelper.CreateRefreshToken(_jwtConfig, userValidateDto)
            });
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<UserInfoDto> GetCurrentUserInfo()
        {
            return await _accountService.GetCurrentUserInfo();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public IActionResult Logout()
        {
            //这个方法可以解析Token信息
            //var Token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            return new OkResult();
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="tokenInfo"><see cref="RefreshTokenInputDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut()]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshTokenInputDto tokenInfo)
        {
            var userValidateDto = await _accountService.GetUserValidateInfo(tokenInfo);

            return new OkObjectResult(new
            {
                Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, userValidateDto, tokenInfo.RefreshToken),
                tokenInfo.RefreshToken
            });
        }

        /// <summary>
        /// 修改登录用户密码
        /// </summary>
        /// <param name="inputDto"><see cref="UserChangePwdInputDto"/></param>
        /// <returns></returns>
        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePwdInputDto inputDto)
        {
            await _accountService.UpdatePassword(inputDto);
            return new OkResult();
        }
    }
}