﻿using Adnc.Application.Shared;
using Adnc.Shared.ConfigModels;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Application.Contracts.Services;
using Adnc.Usr.WebApi.Authorize;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 认证/修改密码/注销
    /// </summary>
    [Route("usr/session")]
    [ApiController]
    public class AccountController : AdncControllerBase
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IUserContext _userContext;
        private readonly IAccountAppService _accountService;

        public AccountController(IOptionsSnapshot<JwtConfig> jwtConfig
            , IAccountAppService accountService
            , IUserContext userContext)
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
            var result = await _accountService.GetUserInfoAsync(_userContext.Id);

            if (result != null)
                return result;

            return NotFound();
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Logout()
            => NoContent();

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
            //这个方法可以解析Token信息
            //var Token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var result = await _accountService.GetUserValidateInfoAsync(input.Id);

            if (result == null)
                return Ok(new UserTokenInfoDto
                {
                    Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, result, input.RefreshToken),
                    RefreshToken = input.RefreshToken
                });

            return NotFound();
        }

        /// <summary>
        /// 修改登录用户密码
        /// </summary>
        /// <param name="input"><see cref="UserChangePwdDto"/></param>
        /// <returns></returns>
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePwdDto input)
            => Result(await _accountService.UpdatePasswordAsync(_userContext.Id, input));
    }
}