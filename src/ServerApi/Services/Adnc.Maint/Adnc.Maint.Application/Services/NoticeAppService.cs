using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoMapper;
using Adnc.Infra.Common.Extensions;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Application.Shared.Services;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;

namespace  Adnc.Maint.Application.Services
{
    public class NoticeAppService : AbstractAppService, INoticeAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysNotice> _noticeRepository;

        public NoticeAppService(IMapper mapper, IEfRepository<SysNotice> noticeRepository)
        {
            _mapper = mapper;
            _noticeRepository = noticeRepository;
        }

        public async Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search)
        {
            Expression<Func<SysNotice, bool>> whereCondition = x => true;
            if (search.Title.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Title==search.Title.Trim());
            }

            var notices = await _noticeRepository.Where(whereCondition).ToListAsync();

            return _mapper.Map<List<NoticeDto>>(notices);
        }
    }
}
