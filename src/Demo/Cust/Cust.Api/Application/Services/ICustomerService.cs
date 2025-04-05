namespace Adnc.Demo.Cust.Api.Application.Services;

public interface ICustomerService : IAppService
{
    /// <summary>
    /// 客户创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "客户创建")]
    Task<ServiceResult<IdDto>> CreateAsync(CustomerCreationDto input);

    /// <summary>
    /// 客户充值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="balance"></param>
    /// <returns></returns>
    [OperateLog(LogName = "客户充值")]
    [UnitOfWork(Distributed = true)]
    Task<ServiceResult<IdDto>> RechargeAsync(long id, decimal balance);

    /// <summary>
    /// 客户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 客户分页列表(raw sql)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(SearchPagedDto input);

    /// <summary>
    /// 客户充值记录分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync(SearchPagedDto input);
}
