﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.WebApi.Helper;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common;
using Adnc.Usr.Application.Services;
using Adnc.WebApi.Shared;


namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 认证/修改密码/注销
    /// </summary>
    [Route("usr/session")]
    [ApiController]
    public class AccountController : AdncControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly UserContext _userContext;
        private readonly IAccountAppService _accountService;

        public AccountController(IOptionsSnapshot<JWTConfig> jwtConfig
            , IAccountAppService accountService
            , UserContext userContext)
        {
            _jwtConfig = jwtConfig.Value;
            _accountService = accountService;
            _userContext = userContext;
        }

        /// <summary>
        /// 登录/验证
        /// </summary>
        /// <param name="input"><see cref="UserLoginDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserTokenInfoDto>> LoginAsync([FromBody] UserLoginDto input)
        {
            var result = await _accountService.LoginAsync(input);

            if (result.IsSuccess)
                return Created($"/usr/session"
                        ,
                        new UserTokenInfoDto
                        {
                            Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, result.Content),
                            RefreshToken = JwtTokenHelper.CreateRefreshToken(_jwtConfig, result.Content)
                        });

            return Problem(result.ProblemDetails);
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserInfoDto>> GetCurrentUserInfoAsync()
        {
            var userId = _userContext.Id;
            var result = await _accountService.GetUserInfoAsync(_userContext.Id);
            return Result(result);
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
        {
            return NoContent();
            //这个方法可以解析Token信息
            //var Token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="input"><see cref="UserRefreshTokenDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserTokenInfoDto>> RefreshAccessTokenAsync([FromBody] UserRefreshTokenDto input)
        {
            var result = await _accountService.GetUserValidateInfoAsync(input.Account);

            if (result.IsSuccess)
                return Ok(new UserTokenInfoDto
                {
                    Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, result.Content, input.RefreshToken),
                    RefreshToken = input.RefreshToken
                });

            return Problem(result.ProblemDetails);
        }

        /// <summary>
        /// 修改登录用户密码
        /// </summary>
        /// <param name="input"><see cref="UserChangePwdDto"/></param>
        /// <returns></returns>
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePwdDto input)
        {
            return Result(await _accountService.UpdatePasswordAsync(_userContext.Id, input));
        }
    }
}