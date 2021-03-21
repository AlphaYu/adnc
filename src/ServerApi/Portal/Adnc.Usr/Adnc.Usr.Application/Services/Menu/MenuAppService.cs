using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EasyCaching.Core;
using AutoMapper;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Common.Helper;
using Adnc.Application.Shared;
using Adnc.Application.Shared.Services;
using Adnc.Infr.Common.Extensions;

namespace Adnc.Usr.Application.Services
{
    public class MenuAppService :AppService,IMenuAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysMenu> _menuRepository;
        private readonly IEfRepository<SysRelation> _relationRepository;
        private readonly IHybridCachingProvider _cache;

        public MenuAppService(IMapper mapper,
            IEfRepository<SysMenu> menuRepository,
            IEfRepository<SysRelation> relationRepository,
            IHybridProviderFactory hybridProviderFactory)
        {
            _mapper = mapper;
            _menuRepository = menuRepository;
            _relationRepository = relationRepository;
            _cache = hybridProviderFactory.GetHybridCachingProvider(EasyCachingConsts.HybridCaching);
        }

        public async Task<AppSrvResult<long>> CreateAsync(MenuCreationDto input)
        {
            var isExistsCode = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Code == input.Code).Any();
            if (isExistsCode)
                return Problem(HttpStatusCode.Forbidden, "该菜单编码已经存在");

            var isExistsName = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Name == input.Name).Any();
            if (isExistsName)
                return Problem(HttpStatusCode.Forbidden, "该菜单名称已经存在");

            var parentMenu = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Code == input.PCode).FirstOrDefault();
            var addDto = ProducePCodes(input, parentMenu);
            var menu = _mapper.Map<SysMenu>(addDto);
            menu.Id = IdGenerater.GetNextId();
            await _menuRepository.InsertAsync(menu);

            return menu.Id;
        }

        public async Task<AppSrvResult> UpdateAsync(long id, MenuUpdationDto input)
        {
            var isExistsCode = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Code == input.Code && x.Id != id).Any();
            if (isExistsCode)
                return Problem(HttpStatusCode.BadRequest, "该菜单编码已经存在");

            var isExistsName = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Name == input.Name && x.Id != id).Any();
            if (isExistsName)
                return Problem(HttpStatusCode.BadRequest, "该菜单名称已经存在");

            var parentMenu = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Code == input.PCode).FirstOrDefault();
            var updateDto = ProducePCodes(input, parentMenu);
            var menu = _mapper.Map<SysMenu>(updateDto);

            menu.Id = id;

            var updatingProps = UpdatingProps<SysMenu>(
                                                        x => x.Code
                                                        , x => x.Component
                                                        , x => x.Hidden
                                                        , x => x.Icon
                                                        , x => x.IsMenu
                                                        , x => x.IsOpen
                                                        , x => x.Levels
                                                        , x => x.Name
                                                        , x => x.Ordinal
                                                        , x => x.PCode
                                                        , x => x.PCodes
                                                        , x => x.Status
                                                        , x => x.Tips
                                                        , x => x.Url
                                                    );

            await _menuRepository.UpdateAsync(menu, updatingProps);

            return AppSrvResult();
        }

        public async Task<AppSrvResult> DeleteAsync(long Id)
        {
            var menu = (await this.GetAllMenusFromCacheAsync()).Where(x => x.Id == Id).FirstOrDefault();
            await _menuRepository.DeleteRangeAsync(x => x.PCodes.Contains($"[{menu.Code}]") || x.Id == Id);

            return AppSrvResult();
        }

        public async Task<AppSrvResult<List<MenuNodeDto>>> GetlistAsync()
        {
            var result = new List<MenuNodeDto>();

            var menus = (await this.GetAllMenusFromCacheAsync()).OrderBy(o => o.Levels).ThenBy(x => x.Ordinal);

            var menuNodes = _mapper.Map<List<MenuNodeDto>>(menus);
            foreach (var node in menuNodes)
            {
                var parentNode = menuNodes.FirstOrDefault(x => x.Code == node.PCode);
                if (parentNode != null)
                {
                    node.ParentId = parentNode.Id;
                }
            }

            var dictNodes = menuNodes.ToDictionary(x => x.Id);
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

        public async Task<AppSrvResult<List<MenuRouterDto>>> GetMenusForRouterAsync(long[] roleIds)
        {
            var result = new List<MenuRouterDto>();
            //所有菜单
            var allMenus = await this.GetAllMenusFromCacheAsync();
            //所有菜单角色关系
            var allRelations = await this.GetAllRelations();
            //角色拥有的菜单Ids
            var menusIds = allRelations.Where(x => roleIds.Contains(x.RoleId.Value)).Select(x => x.MenuId).Distinct();
            //更加菜单Id获取菜单实体
            var menus = allMenus.Where(x => menusIds.Contains(x.Id));

            if (menus.Any())
            {
                var routerMenus = new List<MenuRouterDto>();

                var componentMenus = menus.Where(x => !string.IsNullOrWhiteSpace(x.Component));
                foreach (var menu in componentMenus)
                {
                    var routerMenu = _mapper.Map<MenuRouterDto>(menu);
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
                        node.ParentId = parentNode.Id;
                    }
                }

                var dictNodes = routerMenus.ToDictionary(x => x.Id);
                foreach (var pair in dictNodes.OrderBy(x => x.Value.Ordinal))
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

        public async Task<AppSrvResult<dynamic>> GetMenuTreeListByRoleIdAsync(long roleId)
        {
            var menuIds = (await this.GetAllRelations()).Where(x => x.RoleId.Value == roleId).Select(r => r.MenuId.Value) ?? new List<long>();
            List<ZTreeNodeDto<long, dynamic>> roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();

            var menus = await _menuRepository.Where(w => true).OrderBy(x=>x.Ordinal).ToListAsync();

            foreach (var menu in menus)
            {
                var parentMenu = menus.FirstOrDefault(x => x.Code == menu.PCode);
                ZTreeNodeDto<long, dynamic> node = new ZTreeNodeDto<long, dynamic>
                {
                    Id = menu.Id,
                    PID = parentMenu != null ? parentMenu.Id : 0,
                    Name = menu.Name,
                    Open = parentMenu != null,
                    Checked = menuIds.Contains(menu.Id)
                };
                roleTreeList.Add(node);
            }

            List<Node<long>> nodes = _mapper.Map<List<Node<long>>>(roleTreeList);
            foreach (var node in nodes)
            {
                foreach (var child in nodes)
                {
                    if (child.PID == node.Id)
                    {
                        node.Children.Add(child);
                    }
                }
            }

            var groups = roleTreeList.GroupBy(x => x.PID).Where(x => x.Key > 1);
            foreach(var group in groups)
            {
                roleTreeList.RemoveAll(x => x.Id == group.Key);
            }

            return new
            {
                treeData = nodes.Where(x => x.PID == 0),
                checkedIds = roleTreeList.Where(x => x.Checked && x.PID != 0).Select(x => x.Id)
            };
        }

        private async Task<List<RelationDto>> GetAllRelations()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuRelationCacheKey, async () =>
            {
                var allRelations = await _relationRepository.GetAll(writeDb:true).ToListAsync();
                return _mapper.Map<List<RelationDto>>(allRelations);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        private async Task<List<MenuDto>> GetAllMenusFromCacheAsync()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuListCacheKey, async () =>
            {
                var allMenus = await _menuRepository.GetAll(writeDb: true).OrderBy(x=>x.Ordinal).ToListAsync();
                return _mapper.Map<List<MenuDto>>(allMenus);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        private MenuCreationDto ProducePCodes(MenuCreationDto saveDto, MenuDto parentMenuDto)
        {
            if (saveDto.PCode.IsNullOrWhiteSpace() || saveDto.PCode.EqualsIgnoreCase("0"))
            {
                saveDto.PCode = "0";
                saveDto.PCodes = "[0],";
                saveDto.Levels = 1;
                return saveDto;
            }

            saveDto.Levels = parentMenuDto.Levels + 1;
            saveDto.PCodes = $"{parentMenuDto.PCodes}[{parentMenuDto.Code}]";

            return saveDto;
        }
    }
}
