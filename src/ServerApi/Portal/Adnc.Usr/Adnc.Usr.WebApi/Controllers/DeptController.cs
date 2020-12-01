using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.Application.Services;
using Adnc.Usr.Application.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 部门
    /// </summary>
    [Route("usr/depts")]
    [ApiController]
    public class DeptController : AdncControllerBase
    {
        private readonly IDeptAppService _deptService;

        public DeptController(IDeptAppService deptService)
        {
            _deptService = deptService;
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("deptDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            return Result(await _deptService.Delete(id));
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("deptList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DeptNodeDto>>> GetList()
        {
            return Result(await _deptService.GetList());
        }

        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="saveDto">部门</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("deptAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody] DeptSaveInputDto saveDto)
        {
            return CreatedResult(await _deptService.Add(saveDto));
        }

        /// <summary>
        /// 修改部门
        /// </summary>
        /// <param name="saveDto">部门</param>
        /// <returns></returns>
        [HttpPut]
        [Permission("deptEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<long>> Update([FromBody] DeptSaveInputDto saveDto)
        {
            return Result(await _deptService.Update(saveDto));
        }
    }
}