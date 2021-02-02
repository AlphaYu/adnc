using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Application.Services;
using Adnc.Infr.Common;
using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("usr/users")]
    [ApiController]
    public class UserController : AdncControllerBase
    {
        private readonly IUserAppService _userService;
        private readonly IRoleAppService _roleService;
        private readonly UserContext _userContext;

        public UserController(IUserAppService userService
            , IRoleAppService roleAppService
            , UserContext userContext)
        {
            _userService = userService;
            _roleService = roleAppService;
            _userContext = userContext;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        [HttpGet()]
        [Permission("userList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<UserDto>>> GetPaged([FromQuery]UserSearchDto searchModel)
        {
            return Result(await _userService.GetPaged(searchModel));
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userDto">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("userAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody] UserSaveInputDto userDto)
        {
            return CreatedResult(await _userService.Add(userDto));
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userDto">用户信息</param>
        /// <returns></returns>
        [HttpPut]
        [Permission("userEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update([FromBody] UserSaveInputDto userDto)
        {
            return Result(await _userService.Update(userDto));
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [Permission("userDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long userId)
        {
            return Result(await _userService.Delete(userId));
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleIds">角色</param>
        /// <returns></returns>
        [HttpPut("{userId}/roles")]
        [Permission("userSetRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SetRole([FromRoute] long userId, [FromBody] long[] roleIds)
        {
           return Result(await _userService.SetRole(new RoleSetInputDto { Id = userId, RoleIds = roleIds}));
            
        }

        /// <summary>
        /// 变更用户状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPut("{userId}/status")]
        [Permission("userFreeze")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangeStatus([FromRoute]long userId, [FromBody] SimpleInputDto<int> status)
        {
            return Result(await _userService.ChangeStatus(userId, status.Value));
        }

        /// <summary>
        /// 批量变更用户状态
        /// </summary>
        /// <param name="changeStatusInputDto">用户Ids与状态</param>
        /// <returns></returns>
        [HttpPut("batch/status")]
        [Permission("userFreeze")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangeStatus([FromBody] UserChangeStatusInputDto changeStatusInputDto)
        {
            return Result(await _userService.ChangeStatus(changeStatusInputDto));
        }

        [HttpGet("{id}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> GetCurrenUserPermissions([FromRoute] long id, [FromQuery]string[] permissions)
        {
            var inputDto = new RolePermissionsCheckInputDto()
            {
                RoleIds = _userContext.RoleIds
                ,
                Permissions = permissions
            };
            return Result(await _roleService.GetPermissions(inputDto));
        }
    }
}