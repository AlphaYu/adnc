using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared;

namespace Adnc.Maint.Core.CoreServices
{
    public interface IMaintManagerService : ICoreService
    {
        Task UpdateDicts(SysDict dict, List<SysDict> subDicts, CancellationToken cancellationToken = default);
    }
}
