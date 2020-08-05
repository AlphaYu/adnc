using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Core.Entities;

namespace Adnc.Core.IRepositories
{
    public interface IMenuRepository : IEfRepository<SysMenu>
    {
        Task<List<SysMenu>> GetMenusByRoleIdsAsync(long[] roleIds, bool enabledOnly);
    }
}
