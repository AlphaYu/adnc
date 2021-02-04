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

        public async Task<AppSrvResult<long>> Add(MenuSaveInputDto saveDto)
        {
            var isExistsCode = (await this.GetAllMenusFromCache()).Where(x => x.Code == saveDto.Code).Any();
            if (isExistsCode)
                return Problem(HttpStatusCode.Forbidden, "该菜单编码已经存在");

            var isExistsName = (await this.GetAllMenusFromCache()).Where(x => x.Name == saveDto.Name).Any();
            if (isExistsName)
                return Problem(HttpStatusCode.Forbidden, "该菜单名称已经存在");

            var parentMenu = (await this.GetAllMenusFromCache()).Where(x => x.Code == saveDto.PCode).FirstOrDefault();
            var addDto = ProducePCodes(saveDto, parentMenu);
            var menu = _mapper.Map<SysMenu>(addDto);
            menu.Id = IdGenerater.GetNextId();
            await _menuRepository.InsertAsync(menu);

            return menu.Id;
        }

        public async Task<AppSrvResult> Update(MenuSaveInputDto saveDto)
        {
            var isExistsCode = (await this.GetAllMenusFromCache()).Where(x => x.Code == saveDto.Code && x.Id != saveDto.Id).Any();
            if (isExistsCode)
                return Problem(HttpStatusCode.BadRequest, "该菜单编码已经存在");

            var isExistsName = (await this.GetAllMenusFromCache()).Where(x => x.Name == saveDto.Name && x.Id != saveDto.Id).Any();
            if (isExistsName)
                return Problem(HttpStatusCode.BadRequest, "该菜单名称已经存在");

            var parentMenu = (await this.GetAllMenusFromCache()).Where(x => x.Code == saveDto.PCode).FirstOrDefault();
            var updateDto = ProducePCodes(saveDto, parentMenu);
            var menu = _mapper.Map<SysMenu>(updateDto);
            await _menuRepository.UpdateAsync(menu);

            return DefaultResult();
        }

        public async Task<AppSrvResult> Delete(long Id)
        {
            var menu = (await this.GetAllMenusFromCache()).Where(x => x.Id == Id).FirstOrDefault();
            await _menuRepository.DeleteRangeAsync(x => x.PCodes.Contains($"[{menu.Code}]") || x.Id == Id);

            return DefaultResult();
        }

        public async Task<AppSrvResult<List<MenuNodeDto>>> Getlist()
        {
            var result = new List<MenuNodeDto>();

            var menus = (await this.GetAllMenusFromCache()).OrderBy(o => o.Levels).ThenBy(x => x.Num);

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

        public async Task<AppSrvResult<List<RouterMenuDto>>> GetMenusForRouter(long[] roleIds)
        {
            var result = new List<RouterMenuDto>();
            //所有菜单
            var allMenus = await this.GetAllMenusFromCache();
            //所有菜单角色关系
            var allRelations = await this.GetAllRelations();
            //角色拥有的菜单Ids
            var menusIds = allRelations.Where(x => roleIds.Contains(x.RoleId.Value)).Select(x => x.MenuId).Distinct();
            //更加菜单Id获取菜单实体
            var menus = allMenus.Where(x => menusIds.Contains(x.Id));

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
                        node.ParentId = parentNode.Id;
                    }
                }

                var dictNodes = routerMenus.ToDictionary(x => x.Id);
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

        public async Task<AppSrvResult<dynamic>> GetMenuTreeListByRoleId(long roleId)
        {
            var menuIds = (await this.GetAllRelations()).Where(x => x.RoleId.Value == roleId).Select(r => r.MenuId.Value) ?? new List<long>();
            List<ZTreeNodeDto<long, dynamic>> roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();

            //var menus = await _menuRepository.SelectAsync(m=>m, q => true, q => q.Id, true);
            var menus = await _menuRepository.Where(w => true).OrderByDescending(x => x.Id).ToListAsync();

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

        private async Task<List<MenuDto>> GetAllMenusFromCache()
        {
            var cahceValue = await _cache.GetAsync(EasyCachingConsts.MenuListCacheKey, async () =>
            {
                var allMenus= await _menuRepository.GetAll(writeDb:true).ToListAsync();
                return _mapper.Map<List<MenuDto>>(allMenus);
            }, TimeSpan.FromSeconds(EasyCachingConsts.OneYear));

            return cahceValue.Value;
        }

        private MenuSaveInputDto ProducePCodes(MenuSaveInputDto saveDto, MenuDto parentMenuDto)
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
