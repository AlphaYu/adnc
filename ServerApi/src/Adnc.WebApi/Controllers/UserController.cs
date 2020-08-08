using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Application.Dtos;
using Adnc.Application.Services;
using Adnc.Common.Models;

namespace Adnc.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("sys/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userService;

        public UserController(IUserAppService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        [HttpGet()]
        [Permission("user")]
        public async Task<PageModelDto<UserDto>> GetPaged([FromQuery]UserSearchDto searchModel)
        {
            return await _userService.GetPaged(searchModel);
        }

        /// <summary>
        /// 保存用户(新增/修改)
        /// </summary>
        /// <param name="userDto">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("userAdd", "userEdit")]
        public async Task Save([FromBody] UserSaveInputDto userDto)
        {
            await _userService.Save(userDto);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpDelete("{userid}")]
        [Permission("userDelete")]
        public async Task Delete([FromRoute] long userId)
        {
            await _userService.Delete(userId);
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="roleIds">角色(逗号隔开,如1,3,4,9,30)</param>
        /// <returns></returns>
        [HttpPut("{userid}/roles")]
        [Permission("userSetRole")]
        public async Task SetRole([FromRoute] long userId, [FromBody] long[] roleIds)
        {
            await _userService.SetRole(new RoleSetInputDto { ID = userId, RoleIds = string.Join(",", roleIds) });
        }

        /// <summary>
        /// 变更用户状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        [HttpPut("{userid}/status")]
        [Permission("userFreeze")]
        public async Task ChangeStatus([FromRoute]long userId, [FromBody] SimpleInputDto<int> status)
        {
            await _userService.ChangeStatus(userId);
        }

        /// <summary>
        /// 批量变更用户状态
        /// </summary>
        /// <param name="changeStatusInputDto">用户Ids与状态</param>
        /// <returns></returns>
        [HttpPut("batch/status")]
        [Permission("userFreeze")]
        public async Task ChangeStatus([FromBody] UserChangeStatusInputDto changeStatusInputDto)
        {
            await _userService.ChangeStatus(changeStatusInputDto);
        }
    }
}