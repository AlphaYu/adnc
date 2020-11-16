using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.CoreServices;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using EasyCaching.Core;
using Adnc.Application.Shared;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Usr.Application.Services
{
    public class MenuAppService : IMenuAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysMenu> _menuRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IUsrManagerService _systemManagerService;
        private readonly IHybridCachingProvider _cache;

        public MenuAppService(IMapper mapper,
            IEfRepository<SysMenu> menuRepository,
            IEfRepository<SysRelation> relationRepository,
            IUsrManagerService systemManagerService,
            IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _systemManagerService = systemManagerService;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task Save(MenuSaveInputDto saveDto)
        {
            if (saveDto.ID < 1)
            {
                if (await _menuRepository.ExistAsync(x => x.Code == saveDto.Code))
                {
                    throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "同名菜单已存在"));
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
                var parentMenu = await _menuRepository.FetchAsync(m => new { m.Code, m.PCodes, m.Levels }, x => x.Code == saveDto.PCode);
                saveDto.PCode = parentMenu.Code;
                if (string.Equals(saveDto.Code, saveDto.PCode))
                {
                    throw new BusinessException(new ErrorModel(ErrorCode.Forbidden, "菜单编码冲突"));
                }

                saveDto.Levels = parentMenu.Levels + 1;
                saveDto.PCodes = $"{parentMenu.PCodes}[{parentMenu.Code}]";
            }

            var menu = _mapper.Map<SysMenu>(saveDto);
            if (menu.ID == 0)
            {
                menu.ID = IdGenerater.GetNextId();
                await _menuRepository.InsertAsync(menu);
            }
            else
            {
                await _menuRepository.UpdateAsync(menu);
            }
        }

        public async Task Delete(long Id)
        {
            var menu = await _menuRepository.FetchAsync(m => new { m.ID, m.Code }, x => x.ID == Id);
            await _systemManagerService.DeleteMenu(menu);
        }

        public async Task<List<MenuNodeDto>> Getlist()
        {
            var result = new List<MenuNodeDto>();

            var menus = (await this.GetAllMenusFromCache()).OrderBy(o => o.Levels).ThenBy(x => x.Num);

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

        public async Task<List<RouterMenuDto>> GetMenusForRouter(long[] roleIds)
        {
            var result = new List<RouterMenuDto>();
            //所有菜单
            var allMenus = await this.GetAllMenusFromCache();
            //所有菜单角色关系
            var allRelations = await this.GetAllRelations();
            //角色拥有的菜单Ids
            var menusIds = allRelations.Where(x => roleIds.Contains(x.RoleId.Value)).Select(x => x.MenuId).Distinct();
            //更加菜单Id获取菜单实体
            var menus = allMenus.Where(x => menusIds.Contains(x.ID));

            if (menus.Any())
            {
                var routerMenus = new List<RouterMenuDto>();

                var componentMenus = menus.Where(x => !string.IsNullOrWhiteSpace(x.Component));
                foreach (var menu in componentMenus)
                {
                    var routerMenu = _mapper.Map<RouterMenuDto>(menu);
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
            var menuIds = (await this.GetAllRelations()).Where(x => x.RoleId.Value == roleId).Select(r => r.MenuId.Value) ?? new List<long>();
            List<ZTreeNodeDto<long, dynamic>> roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();

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

        private async Task<List<RelationDto>> GetAllRelations()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuRelationCacheKey, async () =>
            {
                var allRelations = await _relationRepository.GetAll().ToListAsync();
                return _mapper.Map<List<RelationDto>>(allRelations);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        private async Task<List<MenuDto>> GetAllMenusFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuListCacheKey, async () =>
            {
                var allMenus= await _menuRepository.GetAll().ToListAsync();
                return _mapper.Map<List<MenuDto>>(allMenus);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }
    }
}
