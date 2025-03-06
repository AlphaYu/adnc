using Adnc.Demo.Usr.Application.Contracts.Dtos.Menu;

namespace Adnc.Demo.Usr.Application.Services;

public class MenuAppService(IEfRepository<Menu> menuRepo, CacheService cacheService) : AbstractAppService, IMenuAppService
{
    public async Task<AppSrvResult<long>> CreateAsync(MenuCreationDto input)
    {
        input.TrimStringFields();

        var existsCode = await menuRepo.AnyAsync(x => x.Code == input.Code);
        if (existsCode)
            return Problem(HttpStatusCode.BadRequest, "该菜单编码已经存在");

        var existsName = await menuRepo.AnyAsync(x => x.Name == input.Name);
        if (existsName)
            return Problem(HttpStatusCode.BadRequest, "该菜单名称已经存在");

        var menuDto = ProducePCodes(input);
        var menuEntity = Mapper.Map<Menu>(menuDto, IdGenerater.GetNextId());

        await menuRepo.InsertAsync(menuEntity);

        return menuEntity.Id;
    }

    public async Task<AppSrvResult> UpdateAsync(long id, MenuUpdationDto input)
    {
        input.TrimStringFields();

        var existsCode = await menuRepo.AnyAsync(x => x.Code == input.Code && x.Id != id);
        if (existsCode)
            return Problem(HttpStatusCode.BadRequest, "该菜单编码已经存在");

        var existsName = await menuRepo.AnyAsync(x => x.Name == input.Name && x.Id != id);
        if (existsName)
            return Problem(HttpStatusCode.BadRequest, "该菜单名称已经存在");

        var menuEntity = await menuRepo.FetchAsync(x => x.Id == id);
        if (menuEntity is null)
            return Problem(HttpStatusCode.NotFound, "该菜单不存在");

        var menuDto = ProducePCodes(input);
        Mapper.Map(menuDto,menuEntity);

        await menuRepo.UpdateAsync(menuEntity);

        return AppSrvResult();
    }

    public async Task<AppSrvResult> DeleteAsync(long id)
    {
        var menuEnity = await menuRepo.FetchAsync(x => new { x.Code, x.PCodes }, x => x.Id == id);
        if (menuEnity is null)
            return AppSrvResult();

        var needDeletedPCodes = $"{menuEnity.PCodes}[{menuEnity.Code}]";
        await menuRepo.ExecuteDeleteAsync(x => x.PCodes.Contains(needDeletedPCodes) || x.Id == id);
        return AppSrvResult();
    }

    public async Task<List<MenuNodeDto>> GetlistAsync()
    {
        var result = new List<MenuNodeDto>();

        var menus = (await cacheService.GetAllMenusFromCacheAsync()).OrderBy(o => o.Levels).ThenBy(x => x.Ordinal).ToArray();

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
            if (dictNodes.ContainsKey(currentNode.ParentId))
                dictNodes[currentNode.ParentId].Children.Add(currentNode);
            else
                result.Add(currentNode);
        }

        return result;
    }

    public async Task<List<RouterDto>> GetMenusForRouterAsync(IEnumerable<long> roleIds)
    {

        //所有菜单
        var allMenus = await cacheService.GetAllMenusFromCacheAsync();
        //所有菜单角色关系
        var allRelations = await cacheService.GetAllRoleUserRelationsFromCacheAsync();
        //角色拥有的菜单Ids
        var menusIds = allRelations.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.MenuId).Distinct();
        //根据菜单Id获取菜单实体
        var menus = allMenus.Where(x => menusIds.Contains(x.Id) && x.IsMenu && x.Status == true);
        if (menus.IsNullOrEmpty())
            return [];

        List<RouterDto>  GetChildren(string code)
        {
            var children = new List<RouterDto>();
            var pMenus = menus.Where(x => x.PCode == code);
            foreach (var menu in pMenus)
            {
                var router = new RouterDto
                {
                    Name = menu.Code,
                    Path = menu.Url.StartsWith("http") ? string.Empty : menu.Url,
                    Component = menu.Component,
                    Redirect = string.Empty,
                    Meta = new RouterDto.RouteMeta
                    {
                        Icon = menu.Icon,
                        Title = menu.Name,
                        Hidden = menu.Hidden,
                        KeepAlive = true,
                    },
                    Children = GetChildren(menu.Code)
                };
                children.Add(router);
            }
            return children;
        }

        return GetChildren("0");
    }

    public async Task<MenuTreeDto> GetMenuTreeListByRoleIdAsync(long roleId)
    {
        var menuIds = (await cacheService.GetAllRoleUserRelationsFromCacheAsync()).Where(x => x.RoleId == roleId).Select(r => r.MenuId) ?? new List<long>();
        var roleTreeList = new List<ZTreeNodeDto<long, dynamic>>();

        var menus = (await cacheService.GetAllMenusFromCacheAsync()).Where(w => true).OrderBy(x => x.Ordinal);

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

    private async Task<MenuCreationDto> ProducePCodes(MenuCreationDto menuCreationDto)
    {
        var parentMenuEntity = await menuRepo.FetchAsync(x => new { x.Code, x.PCodes, x.Levels }, y => y.Code == menuCreationDto.PCode);
        if (parentMenuEntity is null || menuCreationDto.PCode.Equals("0"))
        {
            menuCreationDto.PCode = "0";
            menuCreationDto.PCodes = "[0]";
            menuCreationDto.Levels = 1;
        }
        else
        {
            menuCreationDto.Levels = parentMenuEntity.Levels + 1;
            menuCreationDto.PCodes = $"{parentMenuEntity.PCodes}[{parentMenuEntity.Code}]";
        }
        return menuCreationDto;
    }
}