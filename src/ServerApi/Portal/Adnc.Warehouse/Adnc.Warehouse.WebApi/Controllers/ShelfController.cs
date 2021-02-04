using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Adnc.Warehouse.Application.Services;
using Adnc.Warehouse.Application.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Infr.Common.Extensions;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.WebApi.Controllers
{
    /// <summary>
    /// 货架管理
    /// </summary>
    [Route("warehouse/shelf")]
    [ApiController]
    public class ShelfController : AdncControllerBase
    {
        private readonly IShelfAppService _shelfSrv;

        public ShelfController(IShelfAppService shelfSrv)
        {
            _shelfSrv = shelfSrv;
        }

        /// <summary>
        /// 新建货架
        /// </summary>
        /// <param name="input"><see cref="ShelfCreationDto"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShelfDto>> CreateAsync([FromBody] ShelfCreationDto input)
        {
            return await _shelfSrv.CreateAsync(input);
        }

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}/product")]
        public async Task<ActionResult<ShelfDto>> AllocateShelfToProductAsync([FromRoute] long id, [FromBody] ShelfAllocateToProductDto input)
        {
            return await _shelfSrv.AllocateShelfToProductAsync(id, input);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModelDto<ShelfDto>> GetPagedAsync([FromQuery]ShlefSearchDto search)
        {
            return await _shelfSrv.GetPagedAsync(search);
        }
    }
}