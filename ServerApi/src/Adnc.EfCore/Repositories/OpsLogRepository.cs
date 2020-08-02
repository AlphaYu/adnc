using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Adnc.Core.Entities;
using Adnc.Core.IRepositories;

namespace  Adnc.Infr.EfCore.Repositories
{
    public class OpsLogRepository : BaseRepository<AdncDbContext, SysOperationLog>, IOpsLogRepository
    {
        public OpsLogRepository(AdncDbContext dbContext) 
            : base(dbContext)
        {
        }

        /// <summary>
        /// 清除操作日志
        /// </summary>
        /// <returns></returns>
        public async Task ClearLogs()
        {
            await DbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE SysOperationLog");
        }
    }
}
