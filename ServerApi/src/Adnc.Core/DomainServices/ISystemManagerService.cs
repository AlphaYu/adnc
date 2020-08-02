using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Core.Entities;

namespace Adnc.Core.DomainServices
{
    public interface ISystemManagerService : IDomainService
    {
        Task<int> AddUser(SysUser user,CancellationToken cancellationToken = default);

        Task<int> SaveRolePermisson(long roleId,long[] permissions,CancellationToken cancellationToken = default);

        Task<int> DeleteMenu(SysMenu menu, CancellationToken cancellationToken = default);

        Task<int> DeleteDept(long deptId, CancellationToken cancellationToken = default);

        Task<int> UpdateDicts(SysDict dict,IEnumerable<SysDict> subDicts, CancellationToken cancellationToken = default);
    }
}
