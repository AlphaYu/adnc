using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Adnc.Application.Shared.Dtos;
using Adnc.Cus.Application.Services;
using Adnc.Cus.Application.Dtos;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Http;

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

        public CustomerController(ICustomerAppService cusService)
        {
            _cusService = cusService;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"><see cref="CustomerRegisterDto"/></param>
        /// <returns></returns>
        [HttpPost]
        //[Permission("customerRegister")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CustomerDto>> RegisterAsync([FromBody][NotNull] CustomerRegisterDto input)
        {
            return CreatedResult(await _cusService.RegisterAsync(input));
        }

        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}/balance")]
        //[Permission("customerRecharge")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SimpleDto<string>>> RechargeAsync([FromRoute] long id, [FromBody] CustomerRechargeDto input)
        {
            return Result(await _cusService.RechargeAsync(id, input));
        }

        /// <summary>
        /// 客户分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("page")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedAsync([FromQuery]CustomerSearchPagedDto search)
        {
            return Result(await _cusService.GetPagedAsync(search));
        }
    }
}