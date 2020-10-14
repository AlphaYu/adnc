using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Maint.Application.Dtos;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{ 
    public interface INoticeAppService : IAppService
    {
        Task<List<NoticeDto>> GetList(string title);
    }
}
