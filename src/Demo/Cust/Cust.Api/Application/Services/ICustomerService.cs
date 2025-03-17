namespace Adnc.Demo.Cust.Api.Application.Services;

public interface ICustomerService : IAppService
{
    /// <summary>
    /// 创建客户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "客户创建")]
    Task<ServiceResult<IdDto>> CreateAsync(CustomerCreationDto input);

    /// <summary>
    /// 充值
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperateLog(LogName = "客户充值")]
    [UnitOfWork(SharedToCap = true)]
    Task<ServiceResult<IdDto>> RechargeAsync(long id, CustomerRechargeDto input);

    /// <summary>
    /// 分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedAsync(SearchPagedDto input);

    /// <summary>
    /// 分页列表(raw sql)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ServiceResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(SearchPagedDto input);
}