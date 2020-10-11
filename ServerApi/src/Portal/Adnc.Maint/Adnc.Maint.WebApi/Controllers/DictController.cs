using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.Common.Helper;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 字典
    /// </summary>
    [Route("maint/dicts")]
    [ApiController]
    public class DictController : ControllerBase
    {
        private readonly IDictAppService _dictService;

        public DictController(IDictAppService dicttService)
        {
            _dictService = dicttService;
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="Id">字典ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("dictDelete")]
        public async Task DeleteDept([FromRoute]long Id)
        {
            await _dictService.Delete(Id);
        }

        /// <summary>
        /// 获取字典列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("dict")]
        public async Task<List<DictDto>> GetDicttList([FromQuery] DictSearchDto searchDto)
        {            
            return await _dictService.GetList(searchDto);
        }

        /// <summary>
        /// 保存字典信息
        /// </summary>
        /// <param name="dictDto">字典</param>
        /// <returns></returns>
        [HttpPost,HttpPut]
        [Permission("dictAdd", "dictEdit")]
        public async Task SaveDept([FromBody]DictSaveInputDto dictDto)
        {
            await _dictService.Save(dictDto);
        }
    }
}