namespace Adnc.Demo.Admin.Repository;

public static class MenuRepositoryExtension
{
    /*
    public static async Task<List<Menu>> GetMenusByRoleIdsAsync(this IEfRepository<Menu> repo, long[] roleIds, bool? status)
    {
        var roleMenus = repo.GetAll<RoleMenuRelation>().Where(r => roleIds.Contains(r.RoleId)).DistinctBy(x => x.MenuId);

        var menuWhere = ExpressionCreator.New<Menu>().AndIf(status is not null, x => x.Status == status);
        var menus = repo.GetAll<Menu>().Where(menuWhere);

        var result = await (from m in menus
                            join rm in roleMenus on m.Id equals rm.MenuId
                            select m).ToListAsync();

        return result ?? [];
    }
    */
}
