using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Adnc.Usr.Application.Services;
using Adnc.Usr.Application.Dtos;
using Adnc.Infr.Common;

namespace Adnc.Usr.WebApi.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Route("usr/menus")]
    [ApiController]
    public class MenuController : ControllerBase
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
        public async Task<List<MenuNodeDto>> GetMenus()
        {
            return await _menuService.Getlist();
        }

        /// <summary>
        /// 获取侧边栏路由菜单
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        [HttpGet("routers")]
        public async Task<List<RouterMenuDto>> GetMenusForRouter()
        {
            return await _menuService.GetMenusForRouter(_userContext.RoleIds);
        }

        /// <summary>
        /// 根据角色获取菜单树
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        [HttpGet("{roleid}/menutree")]
        public async Task<dynamic> GetMenuTreeListByRoleId([FromRoute]long roleId)
        {
            return await _menuService.GetMenuTreeListByRoleId(roleId);
        }

        /// <summary>
        /// 保存菜单信息
        /// </summary>
        /// <param name="menuDto">菜单</param>
        /// <returns></returns>
        [HttpPost]
        [Permission("menuAdd", "menuEdit")]
        public async Task SaveMenu([FromBody]MenuSaveInputDto menuDto)
        {
            await _menuService.Save(menuDto);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuId">菜单ID</param>
        /// <returns></returns>
        [HttpDelete("{menuid}")]
        [Permission("menuDelete")]
        public async Task Delete([FromRoute]long menuId)
        {
            await _menuService.Delete(menuId);
        }
    }
}