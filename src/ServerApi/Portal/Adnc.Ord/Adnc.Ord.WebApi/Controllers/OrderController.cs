﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Ord.Application.Services;
using Adnc.Ord.Application.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Ord.WebApi.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    [Route("ord/orders")]
    [ApiController]
    public class OrderController : AdncControllerBase
    {
        private readonly IOrderAppService _orderSrv;

        public OrderController(IOrderAppService orderSrv)
        {
            _orderSrv = orderSrv;
        }

        /// <summary>
        /// 新建订单
        /// </summary>
        /// <param name="input"><see cref="OrderCreationDto"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] OrderCreationDto input)
        {
            return await _orderSrv.CreateAsync(input);
        }

        /// <summary>
        /// 订单付款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/payment")]
        public async Task<ActionResult> PayAsync([FromRoute] long id)
        {
            await _orderSrv.PayAsync(id);
            return NoContent();
        }

        /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="id"></param>s
        /// <returns></returns>
        [HttpPatch("{id}/status/canceler")]
        public async Task<ActionResult> CancelAsync([FromRoute] long id)
        {
            await _orderSrv.CancelAsync(id);
            return NoContent();
        }

        /// <summary>
        /// 订单删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            await _orderSrv.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetAsync([FromRoute] long id)
        {
            return await _orderSrv.GetAsync(id);
        }

        /// <summary>
        /// 订单分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PageModelDto<OrderDto>>> GetPagedAsync([FromQuery] OrderSearchPagedDto search)
        {
            return await _orderSrv.GetPagedAsync(search);
        }
    }
}