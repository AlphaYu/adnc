using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Adnc.Core.Entities;

namespace Adnc.Core.IRepositories
{
    public interface IOpsLogRepository : IEfRepository<SysOperationLog>
    {
        Task ClearLogs();
    }
}