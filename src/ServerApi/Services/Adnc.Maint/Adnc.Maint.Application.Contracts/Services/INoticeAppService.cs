using System.Collections.Generic;
using System.Threading.Tasks;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Application.Shared.Services;

namespace Adnc.Maint.Application.Contracts.Services
{ 
    public interface INoticeAppService : IAppService
    {
        Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search);
    }
}
