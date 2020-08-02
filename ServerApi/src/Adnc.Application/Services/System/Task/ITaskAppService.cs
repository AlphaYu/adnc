using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Application.Dtos;
using Adnc.Core.Entities;

namespace Adnc.Application.Services
{
    public interface ITaskAppService : IAppService
    {
        Task<List<TaskDto>> GetList(TaskSearchDto searchDto);

        Task<int> Save(TaskSaveInputDto savetDto);

        Task<int> Delete(long Id);

        Task<PageModelDto<TaskLogDto>> GetLogPaged(TaskSearchDto searchDto);
    }
}
