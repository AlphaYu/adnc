using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Whse.Application.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Adnc.Whse.WebApi.Controllers
{
    /// <summary>
    /// 货架管理
    /// </summary>
    [Route("whse/warehouses")]
    [ApiController]
    public class WarehouseController : AdncControllerBase
    {
        private readonly IWarehouseAppService _warehouseSrv;

        public WarehouseController(IWarehouseAppService warehouseSrv)
            => _warehouseSrv = warehouseSrv;

        /// <summary>
        /// 新建货架
        /// </summary>
        /// <param name="input"><see cref="WarehouseCreationDto"/></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateAsync([FromBody] WarehouseCreationDto input)
            => await _warehouseSrv.CreateAsync(input);

        /// <summary>
        /// 分配货架给商品
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}/product")]
        public async Task<ActionResult<WarehouseDto>> AllocateShelfToProductAsync([FromRoute] long id, [FromBody] WarehouseAllocateToProductDto input)
            => await _warehouseSrv.AllocateShelfToProductAsync(id, input);

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PageModelDto<WarehouseDto>> GetPagedAsync([FromQuery] WarehouseSearchDto search)
            => await _warehouseSrv.GetPagedAsync(search);
    }
}