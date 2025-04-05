namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

/// <summary>
/// 通知
/// </summary>
public class NoticeSearchPagedDto : SearchPagedDto
{
    public string? Title { get; set; }

    public int? PublishStatus { get; set; }

    public int? IsRead { get; set; }
}
