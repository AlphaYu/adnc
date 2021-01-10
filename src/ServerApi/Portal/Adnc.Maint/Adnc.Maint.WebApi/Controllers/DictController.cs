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
        private readonly IDictAppService _dictService;

        public DictController(IDictAppService dicttService)
        {
            _dictService = dicttService;
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id">字典ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("dictDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            return Result(await _dictService.Delete(id));
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("dictList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DictDto>>> GetDicttList([FromQuery] DictSearchDto searchDto)
        {
            return Result(await _dictService.GetList(searchDto));
        }

        /// <summary>
        /// 获取单个字典数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Permission("dict")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DictDto>> Get([FromRoute]long id)
        {
            return Result(await _dictService.Get(id));
        }

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="dictDto">字典</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("dictAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody]DictSaveInputDto dictDto)
        {
            return CreatedResult(await _dictService.Add(dictDto));
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="dictDto">字典</param>
        /// <returns></returns>
        [HttpPut]
        [Permission("dictEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<long>> Update([FromBody] DictSaveInputDto dictDto)
        {
            return Result(await _dictService.Update(dictDto));
        }
    }
}