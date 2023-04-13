namespace Adnc.Demo.Usr.Repository
{
    public static class MenuRepositoryExtension
    {
        public static async Task<List<Menu>> GetMenusByRoleIdsAsync(this IEfRepository<Menu> repo, long[] roleIds, bool enabledOnly)
        {
            var query = repo.GetAll<RoleRelation>().Where(r => roleIds.Contains(r.RoleId))
                                       .Select(u => new { u.Menu });
            if (enabledOnly)
                query = query.Where(r => r.Menu.Status);

            var relations = await query.ToListAsync();

            return relations.Select(d => d.Menu).Distinct().ToList();
        }
    }
}