using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Dtos;

namespace Adnc.Application.Services
{ 
    public interface INoticeAppService : IAppService
    {
        Task<List<NoticeDto>> GetList(string title);
    }
}
