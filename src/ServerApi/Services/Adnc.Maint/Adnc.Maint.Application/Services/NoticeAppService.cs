using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infra.Common.Extensions;
using Adnc.Maint.Application.Contracts.Dtos;
using Adnc.Maint.Application.Contracts.Services;
using Adnc.Maint.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adnc.Maint.Application.Services
{
    public class NoticeAppService : AbstractAppService, INoticeAppService
    {
        private readonly IEfRepository<SysNotice> _noticeRepository;

        public NoticeAppService(IEfRepository<SysNotice> noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }

        public async Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search)
        {
            Expression<Func<SysNotice, bool>> whereCondition = x => true;
            if (search.Title.IsNotNullOrWhiteSpace())
            {
                whereCondition = whereCondition.And(x => x.Title == search.Title.Trim());
            }

            var notices = await _noticeRepository.Where(whereCondition).ToListAsync();

            return Mapper.Map<List<NoticeDto>>(notices);
        }
    }
}