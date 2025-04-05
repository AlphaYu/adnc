using Adnc.Demo.Cust.Api.Application.Services;

namespace Adnc.Demo.Cust.Api.Controllers;

/// <summary>
/// 客户管理
/// </summary>
[Route($"{RouteConsts.CustRoot}/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService) : AdncControllerBase
{
    /// <summary>
    /// 创建客户
    /// </summary>
    /// <param name="input"><see cref="CustomerCreationDto"/></param>
    /// <returns><see cref="CustomerDto"/></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Customer.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] CustomerCreationDto input)
        => CreatedResult(await customerService.CreateAsync(input));

    /// <summary>
    /// 后台管理员给客户充值
    /// </summary>
    /// <returns></returns>
    [HttpPatch("{id}/balance")]
    [AdncAuthorize(PermissionConsts.Customer.Recharge)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IdDto>> RechargeAsync([FromRoute] long id, decimal balance)
        => Result(await customerService.RechargeAsync(id, balance));

    /// <summary>
    /// 客户分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{CustomerDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Customer.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => Result(await customerService.GetPagedAsync(input));

    /// <summary>
    /// 客户分页列表-通过Sql查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{CustomerDto}"/></returns>
    [HttpGet("page/rawsql")]
    [AdncAuthorize(PermissionConsts.Customer.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync([FromQuery] SearchPagedDto input)
      => Result(await customerService.GetPagedBySqlAsync(input));

    /// <summary>
    /// 客户充值记录分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{TransactionLogDto}"/></returns>
    [HttpGet("page/transactionlogs")]
    [AdncAuthorize(PermissionConsts.Customer.SearchTransactionLog)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync([FromQuery] SearchPagedDto input)
        => Result(await customerService.GetTransactionLogsPagedAsync(input));
}
