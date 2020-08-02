using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Application.Dtos;
using Adnc.Application.Services;
using Adnc.Common.Models;

namespace Adnc.WebApi.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("sys/roles")]
    [ApiController]
    public class RoleController : ControllerBase
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
        [Permission("role")]
        public async Task<PageModelDto<RoleDto>> GetPaged([FromQuery] RoleSearchDto searchModel)
        {
            return await _roleService.GetPaged(searchModel);
        }

        /// <summary>
        /// 根据用户ID获取角色树
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet("{userid}/rolestree")]
        public async Task<dynamic> GetRoleTreeListByUserId([FromRoute]long userId)
        {
            dynamic result = await _roleService.GetRoleTreeListByUserId(userId);
            if (result == null)
                return new NotFoundObjectResult(new ErrorModel(ErrorCode.NotFound, "没有数据"));
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Id">角色ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("roleDelete")]
        public async Task Delete([FromRoute]long Id)
        {
            await _roleService.Delete(Id);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">用户权限Ids</param>
        /// <returns></returns>
        [HttpPut("{roleid}/permissons")]
        [Permission("roleSetAuthority")]
        public async Task SavePermisson([FromRoute] long roleId,[FromBody]long[] permissions)
        {
            await _roleService.SaveRolePermisson(new PermissonSaveInputDto() { RoleId = roleId, Permissions = permissions });
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="saveDto">角色</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("roleAdd", "roleEdit")]
        public async Task Save([FromBody]RoleSaveInputDto saveDto)
        {
            await _roleService.Save(saveDto);
        }

        [HttpGet("permissions")]
        public async Task<IActionResult> CheckPermissions([FromQuery] RolePermissionsCheckInputDto inputDto)
        {
            bool isexists = await _roleService.ExistPermissions(inputDto);
            if (isexists)
                return new OkResult();
            else
                return new NotFoundObjectResult(new ErrorModel(ErrorCode.NotFound, "没有权限"));
        }
    }
}