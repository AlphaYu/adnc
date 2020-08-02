using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Common;
using Adnc.Common.Models;
using Adnc.Core.DomainServices;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;
using Adnc.Common.Helper;

namespace Adnc.Application.Services
{
    public class MenuAppService : IMenuAppService
    {
        private readonly IMapper _mapper;
        private readonly UserContext _currentUser;
        private readonly IMenuRepository _menuRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly ISystemManagerService _systemManagerService;

        public MenuAppService(IMapper mapper,
            UserContext currentUser,
            IMenuRepository menuRepository,
            IEfRepository<SysRelation> relationRepository,
            ISystemManagerService systemManagerService)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _systemManagerService = systemManagerService;
        }

        public async Task<int> Delete(long Id)
        {
            var menu = await _menuRepository.FetchAsync(m => new { m.ID, m.Code }, x => x.ID == Id);
            return await _systemManagerService.DeleteMenu(menu);
        }

        public async Task<List<MenuNodeDto>> Getlist()
        {
            var result = new List<MenuNodeDto>();
            //var menus = await _menuRepository.GetAsync(q => q.WithPredict(x => true)
            //.WithOrderBy(o => o.OrderBy(x => x.Levels).ThenBy(x => x.Num)));

            var menus = await _menuRepository.GetAll().OrderBy(o => o.Levels).ThenBy(x => x.Num).ToListAsync();

            var menuNodes = _mapper.Map<List<MenuNodeDto>>(menus);
            foreach (var node in menuNodes)
            {
                var parentNode = menuNodes.FirstOrDefault(x => x.Code == node.PCode);
                if (parentNode != null)
                {
                    node.ParentId = parentNode.ID;
                }
            }

            var dictNodes = menuNodes.ToDictionary(x => x.ID);
            foreach (var pair in dictNodes)
            {
                var currentNode = pair.Value;
                if (currentNode.ParentId.HasValue && dictNodes.ContainsKey(currentNode.ParentId.Value))
                {
                    dictNodes[currentNode.ParentId.Value].Children.Add(currentNode);
                }
                else
                {
                    result.Add(currentNode);
                }
            }

            return result;
        }

        public async Task<List<RouterMenuDto>> GetMenusForRouter()
        {
            List<RouterMenuDto> result = new List<RouterMenuDto>();

            var roleIds = _currentUser.RoleId.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
            var menus = await _menuRepository.GetMenusByRoleIdsAsync(roleIds.ToArray(), false);
            if (menus.Any())
            {
                List<RouterMenuDto> routerMenus = new List<RouterMenuDto>();
                foreach (var menu in menus)
                {
                    if (string.IsNullOrWhiteSpace(menu.Component))
                    {
                        continue;
                    }

                    RouterMenuDto routerMenu = _mapper.Map<RouterMenuDto>(menu);
                    routerMenu.Path = menu.Url;
                    routerMenu.Meta = new MenuMetaDto
                    {
                        Icon = menu.Icon,
                        Title = menu.Code
                    };
                    routerMenus.Add(routerMenu);
                }

                foreach (var node in routerMenus)
                {
                    var parentNode = routerMenus.FirstOrDefault(x => x.Code == node.PCode);
                    if (parentNode != null)
                    {
                        node.ParentId = parentNode.ID;
                    }
                }

                var dictNodes = routerMenus.ToDictionary(x => x.ID);
                foreach (var pair in dictNodes.OrderBy(x => x.Value.Num))
                {
                    var currentNode = pair.Value;
                    if (currentNode.ParentId.HasValue && dictNodes.ContainsKey(currentNode.ParentId.Value))
                    {
                        dictNodes[currentNode.ParentId.Value].Children.Add(currentNode);
                    }
                    else
                    {
                        result.Add(currentNode);
                    }
                }
            }

            return result;
        }

        public async Task<dynamic> GetMenuTreeListByRoleId(long roleId)
        {
            var menuIds = await this.GetMenuIdsByRoleId(roleId) ?? new List<long>();
            List<ZTreeNodeDto<long, dynamic>> roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();
            //List<SysMenu> menus = await _menuRepository.GetAsync(q => q.WithOrderBy(o => o.OrderBy(x => x.ID)));

            var menus = await _menuRepository.SelectAsync(m=>m, q => true, q => q.ID, true);


            foreach (var menu in menus)
            {
                var parentMenu = menus.FirstOrDefault(x => x.Code == menu.PCode);
                ZTreeNodeDto<long, dynamic> node = new ZTreeNodeDto<long, dynamic>
                {
                    ID = menu.ID,
                    PID = parentMenu != null ? parentMenu.ID : 0,
                    Name = menu.Name,
                    Open = parentMenu != null,
                    Checked = menuIds.Contains(menu.ID)
                };
                roleTreeList.Add(node);
            }

            List<Node<long>> nodes = _mapper.Map<List<Node<long>>>(roleTreeList);
            foreach (var node in nodes)
            {
                foreach (var child in nodes)
                {
                    if (child.PID == node.ID)
                    {
                        node.Children.Add(child);
                    }
                }
            }

            var groups = roleTreeList.GroupBy(x => x.PID).Where(x => x.Key > 1);
            foreach(var group in groups)
            {
                roleTreeList.RemoveAll(x => x.ID == group.Key);
            }

            return new
            {
                treeData = nodes.Where(x => x.PID == 0),
                checkedIds = roleTreeList.Where(x => x.Checked && x.PID != 0).Select(x => x.ID)
            };
        }

        public async Task<int> Save(MenuSaveInputDto saveDto)
        {
            if (saveDto.ID < 1)
            {
                if (await _menuRepository.ExistAsync(x => x.Code == saveDto.Code))
                {
                    throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"同名菜单已存在"));
                }
                saveDto.Status = true;
            }

            if (string.IsNullOrWhiteSpace(saveDto.PCode) || string.Equals(saveDto.PCode, "0"))
            {
                saveDto.PCode = "0";
                saveDto.PCodes = "[0],";
                saveDto.Levels = 1;
            }
            else
            {
                var parentMenu = await _menuRepository.FetchAsync(m => new { m.Code, m.PCodes,m.Levels }, x => x.Code == saveDto.PCode);
                saveDto.PCode = parentMenu.Code;
                if (string.Equals(saveDto.Code, saveDto.PCode))
                {
                    throw new BusinessException(new ErrorModel(ErrorCode.Forbidden,"菜单编码冲突"));
                }

                saveDto.Levels = parentMenu.Levels + 1;
                saveDto.PCodes = $"{parentMenu.PCodes}[{parentMenu.Code}]";
            }

            var menu = _mapper.Map<SysMenu>(saveDto);
            if (menu.ID == 0)
            {
                menu.ID = IdGeneraterHelper.GetNextId(IdGeneraterKey.MENU);
                return await _menuRepository.InsertAsync(menu);
            }
            else
            {
                return await _menuRepository.UpdateAsync(menu);
            }
        }

        public async Task<List<MenuDto>> GetMenusByRoleIds(long[] roleIds)
        {
            var menus = await _menuRepository.GetMenusByRoleIdsAsync(roleIds, true);

            return _mapper.Map<List<MenuDto>>(menus);
        }

        private async Task<List<long>> GetMenuIdsByRoleId(long roleId)
        {
            return await _relationRepository.SelectAsync(r => r.MenuId, x => x.RoleId == roleId);
        }
    }
}
