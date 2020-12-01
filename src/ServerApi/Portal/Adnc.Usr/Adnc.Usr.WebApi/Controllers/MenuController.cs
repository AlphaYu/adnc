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
            ,UserContext userContext)
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
        public async Task<ActionResult<List<MenuNodeDto>>> GetMenus()
        {
            return Result(await _menuService.Getlist());
        }

        /// <summary>
        /// 获取侧边栏路由菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("routers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<RouterMenuDto>>> GetMenusForRouter()
        {
            return Result(await _menuService.GetMenusForRouter(_userContext.RoleIds));
        }

        /// <summary>
        /// 根据角色获取菜单树
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        [HttpGet("{roleid}/menutree")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<dynamic>> GetMenuTreeListByRoleId([FromRoute]long roleId)
        {
            return Result(await _menuService.GetMenuTreeListByRoleId(roleId));
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="menuDto">菜单</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("menuAdd")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<long>> Add([FromBody] MenuSaveInputDto menuDto)
        {
            return CreatedResult(await _menuService.Add(menuDto));
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="menuDto">菜单</param>
        /// <returns></returns>
        [HttpPut]
        [Permission("menuEdit")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Update([FromBody] MenuSaveInputDto menuDto)
        {
            return Result(await _menuService.Update(menuDto));
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns></returns>
        [HttpDelete("{menuid}")]
        [Permission("menuDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long menuId)
        {
            return Result(await _menuService.Delete(menuId));
        }
    }
}