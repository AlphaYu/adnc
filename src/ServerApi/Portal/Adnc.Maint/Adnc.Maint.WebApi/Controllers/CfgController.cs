using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 配置管理
    /// </summary>
    [Route("maint/cfgs")]
    [ApiController]
    public class CfgController : AdncControllerBase
    {
        private readonly ICfgAppService _cfgAppService;

        public CfgController(ICfgAppService cfgAppService)
        {
            _cfgAppService = cfgAppService;
        }

        /// <summary>
        /// 删除配置节点
        /// </summary>
        /// <param name="Id">ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("cfgDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            return Result(await _cfgAppService.Delete(Id));
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="searchDto"><see cref="CfgSearchDto"/></param>
        /// <returns><see cref="PageModelDto{CfgDto}"/></returns>
        [HttpGet()]
        [Permission("cfgList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<CfgDto>>> GetList([FromQuery] CfgSearchDto searchDto)
        {
            return Result(await _cfgAppService.GetPaged(searchDto));
        }

        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="saveDto"><see cref="CfgSaveInputDto"/></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("cfgAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody] CfgSaveInputDto saveDto)
        {
            return CreatedResult(await _cfgAppService.Add(saveDto));
        }

        /// <summary>
        /// 新增配置
        /// </summary>
        /// <param name="saveDto"><see cref="CfgSaveInputDto"/></param>
        /// <returns></returns>
        [HttpPut]
        [Permission("cfgEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<long>> Update([FromBody] CfgSaveInputDto saveDto)
        {
            return Result(await _cfgAppService.Update(saveDto));
        }
    }
}