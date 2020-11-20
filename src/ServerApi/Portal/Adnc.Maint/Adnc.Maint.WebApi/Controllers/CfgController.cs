using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Maint.Application.Services;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Maint.WebApi.Controllers
{
    /// <summary>
    /// 配置
    /// </summary>
    [Route("maint/cfgs")]
    [ApiController]
    public class CfgController : ControllerBase
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
        public async Task Delete([FromRoute]long Id)
        {
            await _cfgAppService.Delete(Id);
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <param name="searchDto"><see cref="CfgSearchDto"/></param>
        /// <returns><see cref="PageModelDto{CfgDto}"/></returns>
        [HttpGet()]
        [Permission("cfgList")]
        public async Task<PageModelDto<CfgDto>> GetList([FromQuery] CfgSearchDto searchDto)
        {            
            return await _cfgAppService.GetPaged(searchDto);
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="saveDto"><see cref="CfgSaveInputDto"/></param>
        /// <returns></returns>
        [HttpPost]
        [Permission("cfgAdd", "cfgtEdit")]
        public async Task Save([FromBody]CfgSaveInputDto saveDto)
        {
            await _cfgAppService.Save(saveDto);
        }
    }
}