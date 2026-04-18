using Adnc.Demo.Maint.Application.Contracts.Dtos.Log;

namespace Adnc.Demo.Maint.Application.Contracts.Interfaces;

/// <summary>
/// Defines log query services.
/// </summary>
public interface ILogService : IAppService
{
    /// <summary>
    /// Gets a paged list of login logs.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of login logs.</returns>
    Task<PageModelDto<LoginLogDto>> GetLoginLogsPagedAsync(SearchPagedDto input);

    /// <summary>
    /// Gets a paged list of operation logs.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of operation logs.</returns>
    Task<PageModelDto<OperationLogDto>> GetOperationLogsPagedAsync(SearchPagedDto input);

    /*
    /// <summary>
    /// Gets a paged list of NLog entries.
    /// </summary>
    /// <param name="input">The paging and filtering criteria.</param>
    /// <returns>A paged list of NLog entries.</returns>
    Task<PageModelDto<NlogLogDto>> GetNlogLogsPagedAsync(SearchPagedDto input);
    */
}
