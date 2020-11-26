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

        Task UpdateDept(string oldDeptPids,SysDept dept,CancellationToken cancellationToken = default);
    }
}
