namespace Adnc.Cus.Application.Contracts.Services;

public interface ICustomerAppService : IAppService
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "注册")]
    Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto input);

    /// <summary>
    /// 充值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "充值")]
    [UnitOfWork(SharedToCap = true)]
    Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto input);

    /// <summary>
    /// 处理充值
    /// </summary>
    /// <param name="eventDto"></param>
    /// <param name="tracker"></param>
    /// <returns></returns>
    [OperateLog(LogName = "处理充值")]
    [UnitOfWork]
    Task<AppSrvResult> ProcessRechargingAsync(CustomerRechargedEvent eventDto, IMessageTracker tracker);

    /// <summary>
    /// 处理付款
    /// </summary>
    /// <param name="transactionLogId"></param>
    /// <param name="customerId"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    [OperateLog(LogName = "处理付款")]
    [UnitOfWork(SharedToCap = true)]
    Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount);

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search);
}