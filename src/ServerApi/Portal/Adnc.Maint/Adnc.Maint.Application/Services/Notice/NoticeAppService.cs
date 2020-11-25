using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Infr.Common.Extensions;
using Adnc.Maint.Application.Dtos;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;

namespace  Adnc.Maint.Application.Services
{
    public class NoticeAppService : INoticeAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysNotice> _noticeRepository;

        public NoticeAppService(IMapper mapper, IEfRepository<SysNotice> noticeRepository)
        {
            _mapper = mapper;
            _noticeRepository = noticeRepository;
        }

        public async Task<List<NoticeDto>> GetList(string title)
        {
            List<SysNotice> notices = null;
            if (title.IsNullOrEmpty())
            {
                notices = await _noticeRepository.SelectAsync(n => n, x => true);
            }
            else
            {
                notices = await _noticeRepository.SelectAsync(n => n, x => x.Title.Contains(title));
            }

            return _mapper.Map<List<NoticeDto>>(notices);
        }
    }
}
