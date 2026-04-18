using Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;

namespace Adnc.Demo.Cust.Api.Application.Contracts.Interfaces;

public interface ICustomerService : IAppService
{
    /// <summary>
    /// Create a customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "Create customer")]
    Task<ServiceResult<IdDto>> CreateAsync(CustomerCreationDto input);

    /// <summary>
    /// Recharge a customer
    /// </summary>
    /// <param name="id"></param>
    /// <param name="balance"></param>
    /// <returns></returns>
    [OperateLog(LogName = "Recharge customer")]
    [UnitOfWork(Distributed = true)]
    Task<ServiceResult<IdDto>> RechargeAsync(long id, decimal balance);

    /// <summary>
    /// Get a paginated customer list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// Get a paginated customer list (raw SQL)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(SearchPagedDto input);

    /// <summary>
    /// Get a paginated customer recharge log list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync(SearchPagedDto input);
}
