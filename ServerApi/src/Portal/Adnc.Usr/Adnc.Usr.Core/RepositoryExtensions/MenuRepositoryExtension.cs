using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adnc.Usr.Core.RepositoryExtensions
{
    public static class MenuRepositoryExtension
    {
        public static async Task<List<SysMenu>> GetMenusByRoleIdsAsync(this IEfRepository<SysMenu> efRepository, long[] roleIds, bool enabledOnly)
        {
            var query = efRepository.GetAll<SysRelation>().Where(r => roleIds.Contains(r.RoleId))
                            .Select(u => new { u.Menu });
            if (enabledOnly)
                query = query.Where(r => r.Menu.Status == true);


            var relations = await query.ToListAsync();

            return relations.Select(d => d.Menu).Distinct().ToList();
        }
    }
}
