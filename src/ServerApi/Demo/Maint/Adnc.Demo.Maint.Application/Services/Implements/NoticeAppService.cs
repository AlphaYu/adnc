namespace Adnc.Demo.Maint.Application.Services.Implements;

public class NoticeAppService : AbstractAppService, INoticeAppService
{
    private readonly IEfRepository<Notice> _noticeRepository;

    public NoticeAppService(IEfRepository<Notice> noticeRepository)
    {
        _noticeRepository = noticeRepository;
    }

    public async Task<AppSrvResult<List<NoticeDto>>> GetListAsync(NoticeSearchDto search)
    {
        search.TrimStringFields();

        var whereCondition = ExpressionCreator
                                            .New<Notice>()
                                            .AndIf(!string.IsNullOrWhiteSpace(search.Title), x => x.Title == search.Title);

        var notices = await _noticeRepository
                                        .Where(whereCondition)
                                        .ToListAsync();

        return Mapper.Map<List<NoticeDto>>(notices);
    }
}