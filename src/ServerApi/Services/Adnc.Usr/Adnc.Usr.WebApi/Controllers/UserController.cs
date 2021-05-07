using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Application.Shared;
using Adnc.Application.Shared.Dtos;
using Adnc.WebApi.Shared;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Usr.Application.Contracts.Services;

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
        private readonly IUserContext _userContext;

        public UserController(IUserAppService userService, IUserContext userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("userAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] UserCreationDto input)
        {
            return CreatedResult(await _userService.CreateAsync(input));
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">用户信息</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Permission("userEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] UserUpdationDto input)
        {
            return Result(await _userService.UpdateAsync(id, input));
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("userDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            return Result(await _userService.DeleteAsync(id));
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <param name="roleIds">角色</param>
        /// <returns></returns>
        [HttpPut("{id}/roles")]
        [Permission("userSetRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SetRoleAsync([FromRoute] long id, [FromBody] long[] roleIds)
        {
            return Result(await _userService.SetRoleAsync(id, new UserSetRoleDto { RoleIds = roleIds }));
        }

        /// <summary>
        /// 变更用户状态
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPut("{id}/status")]
        [Permission("userFreeze")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangeStatus([FromRoute] long id, [FromBody] Adnc.Application.Shared.Dtos.SimpleDto<int> status)
        {
            return Result(await _userService.ChangeStatusAsync(id, status.Value));
        }

        /// <summary>
        /// 批量变更用户状态
        /// </summary>
        /// <param name="input">用户Ids与状态</param>
        /// <returns></returns>
        [HttpPut("batch/status")]
        [Permission("userFreeze")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> ChangeStatus([FromBody] UserChangeStatusDto input)
        {
            return Result(await _userService.ChangeStatusAsync(input.UserIds, input.Status));
        }

        /// <summary>
        /// 获取当前用户是否拥有指定权限
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        [HttpGet("{id}/permissions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<string>>> GetCurrenUserPermissions([FromRoute] long id, [FromQuery] IEnumerable<string> permissions)
        {
            //throw new System.Exception("测试");
           var result = await _userService.GetPermissionsAsync(_userContext.Id, permissions);
            return result?.Any() == true ? result : new List<string>();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        [HttpGet()]
        [Permission("userList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModelDto<UserDto>>> GetPagedAsync([FromQuery] UserSearchPagedDto search)
        {
            return await _userService.GetPagedAsync(search);
        }
    }
}