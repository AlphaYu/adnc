using Adnc.Application.Shared.Services;
using Adnc.Maint.Application.Contracts.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Contracts.Services
{
    /// <summary>
    /// 通知管理
    /// </summary>
    public interface INoticeAppService : IAppService
    {
        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search);
    }
}