using Adnc.Application.Shared.Dtos;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Application.Contracts.Services;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

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
        /// <param name="input">角色查询条件</param>
        /// <returns></returns>
        [HttpGet()]
        [Permission("roleList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<RoleDto>>> GetPagedAsync([FromQuery] RolePagedSearchDto input)
        {
            return await _roleService.GetPagedAsync(input);
        }

        /// <summary>
        /// 根据用户ID获取角色树
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet("{userId}/rolestree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RoleTreeDto>> GetRoleTreeListByUserIdAsync([FromRoute] long userId)
        {
            return await _roleService.GetRoleTreeListByUserIdAsync(userId);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("roleDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            return Result(await _roleService.DeleteAsync(id));
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <param name="permissions">用户权限Ids</param>
        /// <returns></returns>
        [HttpPut("{id}/permissons")]
        [Permission("roleSetAuthority")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SetPermissonsAsync([FromRoute] long id, [FromBody] long[] permissions)
        {
            return Result(await _roleService.SetPermissonsAsync(new RoleSetPermissonsDto() { RoleId = id, Permissions = permissions }));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input">角色</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("roleAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] RoleCreationDto input)
        {
            return CreatedResult(await _roleService.CreateAsync(input));
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">角色</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Permission("roleEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] RoleUpdationDto input)
        {
            return Result(await _roleService.UpdateAsync(id, input));
        }
    }
}