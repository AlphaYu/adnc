namespace Adnc.Demo.Maint.Application.Services;

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