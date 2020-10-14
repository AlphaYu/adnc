using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Services;
using Adnc.Maint.Core.Entities;

namespace  Adnc.Maint.Application.Services
{
    public interface ITaskAppService : IAppService
    {
        Task<List<TaskDto>> GetList(TaskSearchDto searchDto);

        Task Save(TaskSaveInputDto savetDto);

        Task Delete(long Id);

        Task<PageModelDto<TaskLogDto>> GetLogPaged(TaskSearchDto searchDto);
    }
}
