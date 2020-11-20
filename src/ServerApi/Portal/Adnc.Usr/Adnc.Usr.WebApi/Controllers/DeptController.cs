using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Usr.Application.Services;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 部门
    /// </summary>
    [Route("usr/depts")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        private readonly IDeptAppService _deptService;

        public DeptController(IDeptAppService deptService)
        {
            _deptService = deptService;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="Id">部门ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("deptDelete")]
        public async Task Delete([FromRoute]long id)
        {
            await _deptService.Delete(id);
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("deptList")]
        public async Task<List<DeptNodeDto>> GetList()
        {            
            return await _deptService.GetList();
        }

        /// <summary>
        /// 保存部门信息
        /// </summary>
        /// <param name="saveDto">部门</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("deptAdd", "deptEdit")]
        public async Task Save([FromBody]DeptSaveInputDto saveDto)
        {
            await _deptService.Save(saveDto);
        }
    }
}