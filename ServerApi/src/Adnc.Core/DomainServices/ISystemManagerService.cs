using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Entities;

namespace Adnc.Core.DomainServices
{
    public interface ISystemManagerService : IDomainService
    {
        Task AddUser(SysUser user,CancellationToken cancellationToken = default);

        Task SaveRolePermisson(long roleId,long[] permissions,CancellationToken cancellationToken = default);

        Task DeleteMenu(SysMenu menu, CancellationToken cancellationToken = default);

        Task DeleteDept(long deptId, CancellationToken cancellationToken = default);

        Task UpdateDicts(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default);
    }
}
