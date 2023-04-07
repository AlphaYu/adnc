namespace Adnc.Maint.Application.Services;

public class NoticeAppService : AbstractAppService, INoticeAppService
{
    private readonly IEfRepository<Notice> _noticeRepository;

    public NoticeAppService(IEfRepository<Notice> noticeRepository)
    {
        _noticeRepository = noticeRepository;
    }

    public async Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search)
    {
        var whereCondition = ExpressionCreator
                                            .New<Notice>()
                                            .AndIf(search.Title.IsNotNullOrWhiteSpace(), x => x.Title == search.Title.Trim());

        var notices = await _noticeRepository
                                        .Where(whereCondition)
                                        .ToListAsync();

        return Mapper.Map<List<NoticeDto>>(notices);
    }
}