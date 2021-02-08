using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 字典管理
    /// </summary>
    [Route("maint/dicts")]
    [ApiController]
    public class DictController : AdncControllerBase
    {
        private readonly IDictAppService _dictAppService;

        public DictController(IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("dictDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            return Result(await _dictAppService.DeleteAsync(id));
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("dictList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DictDto>>> GetListAsync([FromQuery] DictSearchDto search)
        {
            return Result(await _dictAppService.GetListAsync(search));
        }

        /// <summary>
        /// 获取单个字典数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Permission("dict")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DictDto>> GetAsync([FromRoute] long id)
        {
            return Result(await _dictAppService.GetAsync(id));
        }

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="input"><see cref="DictCreationDto"/></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("dictAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] DictCreationDto input)
        {
            return CreatedResult(await _dictAppService.CreateAsync(input));
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input"><see cref="DictUpdationDto"/></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Permission("dictEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<long>> UpdateAsync([FromRoute] long id, [FromBody] DictUpdationDto input)
        {
            return Result(await _dictAppService.UpdateAsync(id, input));
        }
    }
}