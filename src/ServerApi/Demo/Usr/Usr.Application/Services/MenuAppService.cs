namespace Adnc.Demo.Usr.Application.Services;

public class MenuAppService : AbstractAppService, IMenuAppService
{
    private readonly IEfRepository<Menu> _menuRepository;
    private readonly CacheService _cacheService;

    public MenuAppService(IEfRepository<Menu> menuRepository, CacheService cacheService)
    {
        _menuRepository = menuRepository;
        _cacheService = cacheService;
    }

    public async Task<AppSrvResult<long>> CreateAsync(MenuCreationDto input)
    {
        input.TrimStringFields();
        var allMenus = await _cacheService.GetAllMenusFromCacheAsync();

        var isExistsCode = allMenus.Any(x => x.Code == input.Code);
        if (isExistsCode)
            return Problem(HttpStatusCode.Forbidden, "该菜单编码已经存在");

        var isExistsName = allMenus.Any(x => x.Name == input.Name);
        if (isExistsName)
            return Problem(HttpStatusCode.Forbidden, "该菜单名称已经存在");

        var parentMenu = allMenus.FirstOrDefault(x => x.Code == input.PCode);
        var addDto = ProducePCodes(input, parentMenu);
        var menu = Mapper.Map<Menu>(addDto);
        menu.Id = IdGenerater.GetNextId();
        await _menuRepository.InsertAsync(menu);

        return menu.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, MenuUpdationDto input)
    {
        input.TrimStringFields();
        var allMenus = await _cacheService.GetAllMenusFromCacheAsync();

        var isExistsCode = allMenus.Any(x => x.Code == input.Code && x.Id != id);
        if (isExistsCode)
            return Problem(HttpStatusCode.BadRequest, "该菜单编码已经存在");

        var isExistsName = allMenus.Any(x => x.Name == input.Name && x.Id != id);
        if (isExistsName)
            return Problem(HttpStatusCode.BadRequest, "该菜单名称已经存在");

        var parentMenu = allMenus.FirstOrDefault(x => x.Code == input.PCode);
        var updateDto = ProducePCodes(input, parentMenu);
        var menu = Mapper.Map<Menu>(updateDto);

        menu.Id = id;

        var updatingProps = UpdatingProps<Menu>(
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

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        var menu = (await _cacheService.GetAllMenusFromCacheAsync()).FirstOrDefault(x => x.Id == id);
        await _menuRepository.DeleteRangeAsync(x => x.PCodes.Contains($"[{menu.Code}]") || x.Id == id);

        return AppSrvResult();
    }

    public async Task<List<MenuNodeDto>> GetlistAsync()
    {
        var result = new List<MenuNodeDto>();

        var menus = (await _cacheService.GetAllMenusFromCacheAsync()).OrderBy(o => o.Levels).ThenBy(x => x.Ordinal).ToArray();

        var menuNodes = Mapper.Map<List<MenuNodeDto>>(menus);
        foreach (var node in menuNodes)
        {
            var parentNode = menuNodes.FirstOrDefault(x => x.Code == node.PCode);
            if (parentNode != null)
                node.ParentId = parentNode.Id;
        }

        var dictNodes = menuNodes.ToDictionary(x => x.Id);
        foreach (var pair in dictNodes)
        {
            var currentNode = pair.Value;
            if (currentNode.ParentId.HasValue && dictNodes.ContainsKey(currentNode.ParentId.Value))
                dictNodes[currentNode.ParentId.Value].Children.Add(currentNode);
            else
                result.Add(currentNode);
        }

        return result;
    }

    public async Task<List<MenuRouterDto>> GetMenusForRouterAsync(IEnumerable<long> roleIds)
    {
        var result = new List<MenuRouterDto>();
        //所有菜单
        var allMenus = await _cacheService.GetAllMenusFromCacheAsync();
        //所有菜单角色关系
        var allRelations = await _cacheService.GetAllRelationsFromCacheAsync();
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
                var routerMenu = Mapper.Map<MenuRouterDto>(menu);
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
                    node.ParentId = parentNode.Id;
            }

            var dictNodes = routerMenus.ToDictionary(x => x.Id);
            foreach (var pair in dictNodes.OrderBy(x => x.Value.Ordinal))
            {
                var currentNode = pair.Value;
                if (currentNode.ParentId.HasValue && dictNodes.ContainsKey(currentNode.ParentId.Value))
                    dictNodes[currentNode.ParentId.Value].Children.Add(currentNode);
                else
                    result.Add(currentNode);
            }
        }

        return result;
    }

    public async Task<MenuTreeDto> GetMenuTreeListByRoleIdAsync(long roleId)
    {
        var menuIds = (await _cacheService.GetAllRelationsFromCacheAsync()).Where(x => x.RoleId.Value == roleId).Select(r => r.MenuId.Value) ?? new List<long>();
        var roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();

        var menus = (await _cacheService.GetAllMenusFromCacheAsync()).Where(w => true).OrderBy(x => x.Ordinal);

        foreach (var menu in menus)
        {
            var parentMenu = menus.FirstOrDefault(x => x.Code == menu.PCode);
            var node = new ZTreeNodeDto<long, dynamic>
            {
                Id = menu.Id,
                PID = parentMenu != null ? parentMenu.Id : 0,
                Name = menu.Name,
                Open = parentMenu != null,
                Checked = menuIds.Contains(menu.Id)
            };
            roleTreeList.Add(node);
        }

        var nodes = Mapper.Map<List<Node<long>>>(roleTreeList);
        foreach (var node in nodes)
        {
            foreach (var child in nodes)
            {
                if (child.PID == node.Id)
                    node.Children.Add(child);
            }
        }

        var groups = roleTreeList.GroupBy(x => x.PID).Where(x => x.Key > 1);
        foreach (var group in groups)
        {
            roleTreeList.RemoveAll(x => x.Id == group.Key);
        }

        return new MenuTreeDto
        {
            TreeData = nodes.Where(x => x.PID == 0),
            CheckedIds = roleTreeList.Where(x => x.Checked && x.PID != 0).Select(x => x.Id)
        };
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