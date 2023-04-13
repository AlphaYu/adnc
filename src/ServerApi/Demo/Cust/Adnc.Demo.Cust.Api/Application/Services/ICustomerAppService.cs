using Adnc.Demo.Cust.Api.Application.Dtos;

namespace Adnc.Demo.Cust.Api.Application.Services;

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
    /// 分页列表
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedAsync(CustomerSearchPagedDto search);

    /// <summary>
    /// 分页列表(raw sql)
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    Task<AppSrvResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync(CustomerSearchPagedDto search);
}