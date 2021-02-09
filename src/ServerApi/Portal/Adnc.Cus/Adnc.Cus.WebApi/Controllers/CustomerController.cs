using System;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.RpcServices;
using Adnc.Cus.Application.Services;
using Adnc.Cus.Application.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.RpcServices.Rtos;

namespace Adnc.Cus.WebApi.Controllers
{
    /// <summary>
    /// 客户管理
    /// </summary>
    [Route("cus/customer")]
    [ApiController]
    public class CustomerController : AdncControllerBase
    {
        private readonly ICustomerAppService _cusService;
        private readonly IMaintRpcService _maintRpcServcie;
        private readonly IAuthRpcService _authRpcServcie;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomerController(ICustomerAppService cusService
            , IMaintRpcService maintRpcServcie
            , IHttpContextAccessor contextAccessor
            , IAuthRpcService authRpcServices)
        {
            _maintRpcServcie = maintRpcServcie;
            _cusService = cusService;
            _contextAccessor = contextAccessor;
            _authRpcServcie = authRpcServices;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="inputDto"><see cref="CustomerRegisterDto"/></param>
        /// <returns></returns>
        [HttpPost]
        //[Permission("customerRegister")]
        public async Task<ActionResult<SimpleDto<string>>> Register([FromBody][NotNull] CustomerRegisterDto inputDto)
        {
            return Result(await _cusService.Register(inputDto));
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/balance")]
        //[Permission("customerRecharge")]
        public async Task<ActionResult<SimpleDto<string>>> Recharge([FromRoute] string id, [FromBody] SimpleInputDto<decimal> inputDto)
        {
            return Result(await _cusService.Recharge(new CustomerRechargeDto { ID = long.Parse(id), Amount = inputDto.Value }));
        }

        /// <summary>
        /// 指定交易记录id查询结果
        /// </summary>
        /// <param name="id">交易id</param>
        /// <returns></returns>
        [HttpGet("tranlogs/{id}")]
        //[Permission("customerRecharge")]
        public async Task<ActionResult<SimpleDto<bool>>> GetRechargedStatus([FromRoute] string id)
        {
            return await new ValueTask<SimpleDto<bool>>(new SimpleDto<bool> { Value = true });
        }

        [HttpGet("testrpc")]
        [AllowAnonymous]
        public async Task<ActionResult<DictRto>> TestCallRpcService()
        {
            var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            if (jwtToken == null)
            {
                var authRpcResult = await _authRpcServcie.LoginAsync(new LoginRto { Account = "alpha2008", Password = "alpha2008" });
                if (authRpcResult.IsSuccessStatusCode)
                    jwtToken = authRpcResult.Content.Token;
                return NotFound();
            }
            var dictRpcResult = await _maintRpcServcie.GetDictAsync(29);
            if (dictRpcResult.IsSuccessStatusCode)
                return dictRpcResult.Content;

            var apiError = ((Refit.ValidationApiException)dictRpcResult.Error).Content;
            return Problem(apiError.Detail, dictRpcResult.Error.Uri.ToString(), apiError.Status, apiError.Title, apiError.Type);
        }
    }
}