namespace Adnc.Cus.Application.Contracts.Services;

public interface ICustomerAppService : IAppService
{
    [OperateLog(LogName = "注册")]
    Task<AppSrvResult<CustomerDto>> RegisterAsync(CustomerRegisterDto input);

    [OperateLog(LogName = "充值")]
    [UnitOfWork(SharedToCap = true)]
    Task<AppSrvResult<SimpleDto<string>>> RechargeAsync(long id, CustomerRechargeDto input);

    [OperateLog(LogName = "处理充值")]
    [UnitOfWork]
    Task<AppSrvResult> ProcessRechargingAsync(CustomerRechargedEvent eventDto, IMessageTracker tracker);

    [OperateLog(LogName = "处理付款")]
    [UnitOfWork(SharedToCap = true)]
    Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount);

    Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search);
}