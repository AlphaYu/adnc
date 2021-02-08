using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Adnc.Usr.Application.Services;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Route("usr/menus")]
    [ApiController]
    public class MenuController : AdncControllerBase
    {
        private readonly IMenuAppService _menuService;
        private readonly UserContext _userContext;

        public MenuController(IMenuAppService menuService
            , UserContext userContext)
        {
            _menuService = menuService;
            _userContext = userContext;
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Permission("menuList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MenuNodeDto>>> GetlistAsync()
        {
            return Result(await _menuService.GetlistAsync());
        }

        /// <summary>
        /// 获取侧边栏路由菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("routers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MenuRouterDto>>> GetMenusForRouterAsync()
        {
            return Result(await _menuService.GetMenusForRouterAsync(_userContext.RoleIds));
        }

        /// <summary>
        /// 根据角色获取菜单树
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        [HttpGet("{roleId}/menutree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<dynamic>> GetMenuTreeListByRoleIdAsync([FromRoute] long roleId)
        {
            return Result(await _menuService.GetMenuTreeListByRoleIdAsync(roleId));
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menuDto">菜单</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("menuAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> CreateAsync([FromBody] MenuCreationDto menuDto)
        {
            return CreatedResult(await _menuService.CreateAsync(menuDto));
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="input">菜单</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Permission("menuEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateAsync([FromRoute] long id, [FromBody] MenuUpdationDto input)
        {
            return Result(await _menuService.UpdateAsync(id, input));
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">菜单ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Permission("menuDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id)
        {
            return Result(await _menuService.DeleteAsync(id));
        }
    }
}