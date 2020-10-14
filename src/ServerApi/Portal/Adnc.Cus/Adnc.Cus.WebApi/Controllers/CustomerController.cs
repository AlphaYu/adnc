using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Cus.Application.Services;
using Adnc.Cus.Application.Dtos;
using Adnc.Application.Shared.Dtos;

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

        public CustomerController(ICustomerAppService cusService)
        {
            _cusService = cusService;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="inputDto"><see cref="RegisterInputDto"/></param>
        /// <returns></returns>
        [HttpPost]
        //[Permission("customerRegister")]
        public async Task Register([FromBody][NotNull] RegisterInputDto inputDto)
        {
            await _cusService.Register(inputDto);
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/balance")]
        //[Permission("customerRecharge")]
        public async Task<SimpleDto<string>> Recharge([FromRoute]string id,[FromBody] SimpleInputDto<decimal> inputDto)
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
    }
}