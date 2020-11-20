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
using Adnc.Cus.Application.RpcServices;

namespace Adnc.Cus.WebApi.Controllers
{
    /// <summary>
    /// 客户管理
    /// </summary>
    [Route("cus/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
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
        /// <param name="inputDto"><see cref="RegisterInputDto"/></param>
        /// <returns></returns>
        [HttpPost]
        //[Permission("customerRegister")]
        public async Task<SimpleDto<string>> Register([FromBody][NotNull] RegisterInputDto inputDto)
        {
            return await _cusService.Register(inputDto);
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/balance")]
        //[Permission("customerRecharge")]
        public async Task<SimpleDto<string>> Recharge([FromRoute] string id, [FromBody] SimpleInputDto<decimal> inputDto)
        {
            return await _cusService.Recharge(new RechargeInputDto { ID = long.Parse(id), Amount = inputDto.Value });
        }

        /// <summary>
        /// 指定交易记录id查询结果
        /// </summary>
        /// <param name="id">交易id</param>
        /// <returns></returns>
        [HttpGet("tranlogs/{id}")]
        //[Permission("customerRecharge")]
        public async Task<SimpleDto<bool>> GetRechargedStatus([FromRoute] string id)
        {
            return await new ValueTask<SimpleDto<bool>>(new SimpleDto<bool> { Result = true });
        }

        [HttpGet("testrpc")]
        [AllowAnonymous]
        public async Task<GetDictReply> TestCallRpcService()
        {
            GetDictReply result;
            var jwtToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");
            try
            {
                if (jwtToken == null)
                {
                    var rpcResult = await _authRpcServcie.Login(new LoginRequest { Account = "alpha2008", Password = "alpha2008" });

                    jwtToken = rpcResult.Content.Token;
                }
                result = await _maintRpcServcie.GetDict($"Bearer {jwtToken}", 29);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }
    }
}