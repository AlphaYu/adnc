using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;
using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("usr/roles")]
    [ApiController]
    public class RoleController : AdncControllerBase
    {
        private readonly IRoleAppService _roleService;

        public RoleController(IRoleAppService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="searchModel">角色查询条件</param>
        /// <returns></returns>
        [HttpGet()]
        [Permission("roleList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<RoleDto>>> GetPaged([FromQuery] RoleSearchDto searchModel)
        {
            return Result(await _roleService.GetPaged(searchModel));
        }

        /// <summary>
        /// 根据用户ID获取角色树
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet("{userid}/rolestree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<dynamic>> GetRoleTreeListByUserId([FromRoute]long userId)
        {
             return Result(await _roleService.GetRoleTreeListByUserId(userId));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Id">角色ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("roleDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute]long Id)
        {
            return Result(await _roleService.Delete(Id));
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">用户权限Ids</param>
        /// <returns></returns>
        [HttpPut("{roleid}/permissons")]
        [Permission("roleSetAuthority")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SavePermisson([FromRoute] long roleId,[FromBody]long[] permissions)
        {
            return Result(await _roleService.SaveRolePermisson(new PermissonSaveInputDto() { RoleId = roleId, Permissions = permissions }));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="saveDto">角色</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("roleAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody] RoleSaveInputDto saveDto)
        {
            return CreatedResult(await _roleService.Add(saveDto));
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="saveDto">角色</param>
        /// <returns></returns>
        [HttpPut]
        [Permission("roleEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update([FromBody] RoleSaveInputDto saveDto)
        {
            return Result(await _roleService.Update(saveDto));
        }
    }
}