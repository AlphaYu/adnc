using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Adnc.WebApi.WebUtiliy;
using Adnc.Common.Models;
using Adnc.Application.Services;
using Adnc.Application.Dtos;

namespace Adnc.AuthApi.Controllers
{
    /// <summary>
    /// 验证/授权/注销
    /// </summary>
    [Route("auth/session")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IAccountAppService _accountService;

        public AccountController(IOptions<JWTConfig> jwtConfig
            , IAccountAppService accountService)
        {
            _jwtConfig = jwtConfig.Value;
            _accountService = accountService;
        }

        /// <summary>
        /// 登录/验证
        /// </summary>
        /// <param name="userDto"><see cref="UserValidateInputDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login([FromBody] UserValidateInputDto userDto)
        {
            var userValidateDto = await _accountService.Login(userDto);

            return new OkObjectResult(new
            {
                Token = JwtTokenUtil.CreateAccessToken(_jwtConfig, userValidateDto),
                RefreshToken = JwtTokenUtil.CreateRefreshToken(_jwtConfig, userValidateDto)
            });
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
                Token = JwtTokenUtil.CreateAccessToken(_jwtConfig, userValidateDto, tokenInfo.RefreshToken),
                tokenInfo.RefreshToken
            });
        }
    }
}