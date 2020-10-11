using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Usr.Core.CoreServices
{
    public interface IUsrManagerService : ICoreService
    {
        Task AddUser(SysUser user,CancellationToken cancellationToken = default);

        Task SaveRolePermisson(long roleId,long[] permissions,CancellationToken cancellationToken = default);

        Task DeleteMenu(SysMenu menu, CancellationToken cancellationToken = default);

        Task DeleteDept(long deptId, CancellationToken cancellationToken = default);

        //Task UpdateDicts(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default);
    }
}
