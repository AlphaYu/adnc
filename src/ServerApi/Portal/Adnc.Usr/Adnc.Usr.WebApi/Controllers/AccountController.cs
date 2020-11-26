using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.WebApi.Helper;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common;
using Adnc.Usr.Application.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 验证/授权/注销
    /// </summary>
    [Route("usr/session")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly UserContext _userContext;
        private readonly IAccountAppService _accountService;
        private readonly ILogger<AccountController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(IOptionsSnapshot<JWTConfig> jwtConfig
            , IAccountAppService accountService
            , ILogger<AccountController> logger
            ,UserContext userContext
            , IHttpContextAccessor contextAccessor)
        {
            _jwtConfig = jwtConfig.Value;
            _accountService = accountService;
            _logger = logger;
            _userContext = userContext;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 登录/验证
        /// </summary>
        /// <param name="userDto"><see cref="UserValidateInputDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<UserTokenInfoDto> Login([FromBody]UserValidateInputDto userDto)
        {
            var userValidateDto = await _accountService.Login(userDto);

            return new UserTokenInfoDto
            {
                Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, userValidateDto),
                RefreshToken = JwtTokenHelper.CreateRefreshToken(_jwtConfig, userValidateDto)
            };
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<UserInfoDto> GetCurrentUserInfo()
        {
            return await _accountService.GetUserInfo(_userContext.ID);
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
        public async Task<UserTokenInfoDto> RefreshAccessToken([FromBody] RefreshTokenInputDto tokenInfo)
        {
            var userValidateDto = await _accountService.GetUserValidateInfo(tokenInfo);

            return new UserTokenInfoDto
            {
                Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, userValidateDto, tokenInfo.RefreshToken),
                RefreshToken = tokenInfo.RefreshToken
            };
        }

        /// <summary>
        /// 修改登录用户密码
        /// </summary>
        /// <param name="inputDto"><see cref="UserChangePwdInputDto"/></param>
        /// <returns></returns>
        [HttpPut("password")]
        public async Task<SimpleDto<bool>> ChangePassword([FromBody] UserChangePwdInputDto inputDto)
        {
            await _accountService.UpdatePassword(inputDto, _userContext.ID);
            return new SimpleDto<bool>
            {
                Result = true
            };
        }
    }
}