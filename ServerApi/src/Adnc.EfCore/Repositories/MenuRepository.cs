using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;

namespace  Adnc.Infr.EfCore.Repositories
{
    public class MenuRepository : BaseRepository<AdncDbContext,SysMenu>, IMenuRepository
    {
        public MenuRepository(AdncDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task<List<SysMenu>> GetMenusByRoleIdsAsync(long[] roleIds, bool enabledOnly)
        {
            var query = DbContext.Set<SysRelation>()
                            .Where(r => roleIds.Contains(r.RoleId))
                            .Select(u => new { u.Menu })
                            .Distinct();

            if (enabledOnly)
                query = query.Where(r => r.Menu.Status == true);


            var relations = await query.ToListAsync();

            return relations.Select(d => d.Menu).ToList();
        }

        public List<SysMenu> GetMenusByRoleIds(long[] roleIds, bool enabledOnly)
        {
            var query = DbContext.Set<SysRelation>()
                            .Where(r => roleIds.Contains(r.RoleId))
                            .Select(u => new { u.Menu })
                            .Distinct();

            if (enabledOnly)
                query = query.Where(r => r.Menu.Status == true);


            var relations = query.ToList();

            return relations.Select(d => d.Menu).ToList();
        }
    }
}
