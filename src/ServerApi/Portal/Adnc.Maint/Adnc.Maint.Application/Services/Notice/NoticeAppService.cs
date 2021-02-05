using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Adnc.Infr.Common.Extensions;
using Adnc.Maint.Application.Dtos;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Services;

namespace  Adnc.Maint.Application.Services
{
    public class NoticeAppService : AppService, INoticeAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysNotice> _noticeRepository;

        public NoticeAppService(IMapper mapper, IEfRepository<SysNotice> noticeRepository)
        {
            _mapper = mapper;
            _noticeRepository = noticeRepository;
        }

        public async Task<AppSrvResult<List<NoticeDto>>> GetList(string title)
        {
            List<SysNotice> notices = null;
            if (title.IsNullOrEmpty())
            {
                notices = await _noticeRepository.Where(x => true).ToListAsync();
                //notices = await _noticeRepository.SelectAsync(n => n, x => true);
            }
            else
            {
                notices = await _noticeRepository.Where(x => x.Title.Contains(title)).ToListAsync();
                //notices = await _noticeRepository.SelectAsync(n => n, x => x.Title.Contains(title));
            }

            return _mapper.Map<List<NoticeDto>>(notices);
        }
    }
}
