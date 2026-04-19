using Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;

namespace Adnc.Demo.Cust.Api.Controllers;

/// <summary>
/// Customer management
/// </summary>
[Route($"{RouteConsts.CustRoot}/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService) : AdncControllerBase
{
    /// <summary>
    /// Create a customer
    /// </summary>
    /// <param name="input"><see cref="CustomerCreationDto"/></param>
    /// <returns><see cref="CustomerDto"/></returns>
    [HttpPost]
    [AdncAuthorize(PermissionConsts.Customer.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<IdDto>> CreateAsync([FromBody] CustomerCreationDto input)
        => CreatedResult(await customerService.CreateAsync(input));

    /// <summary>
    /// Recharge a customer as a back-office admin
    /// </summary>
    /// <returns></returns>
    [HttpPatch("{id}/balance")]
    [AdncAuthorize(PermissionConsts.Customer.Recharge)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IdDto>> RechargeAsync([FromRoute] long id, decimal balance)
        => Result(await customerService.RechargeAsync(id, balance));

    /// <summary>
    /// Get a paginated customer list
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{CustomerDto}"/></returns>
    [HttpGet("page")]
    [AdncAuthorize(PermissionConsts.Customer.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedAsync([FromQuery] SearchPagedDto input)
        => await customerService.GetPagedAsync(input);

    /// <summary>
    /// Get a paginated customer list via SQL
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{CustomerDto}"/></returns>
    [HttpGet("page/rawsql")]
    [AdncAuthorize(PermissionConsts.Customer.Search)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<CustomerDto>>> GetPagedBySqlAsync([FromQuery] SearchPagedDto input)
      => await customerService.GetPagedBySqlAsync(input);

    /// <summary>
    /// Get a paginated customer recharge log list
    /// </summary>
    /// <param name="input"></param>
    /// <returns><see cref="PageModelDto{TransactionLogDto}"/></returns>
    [HttpGet("page/transactionlogs")]
    [AdncAuthorize(PermissionConsts.Customer.SearchTransactionLog)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PageModelDto<TransactionLogDto>>> GetTransactionLogsPagedAsync([FromQuery] SearchPagedDto input)
        => await customerService.GetTransactionLogsPagedAsync(input);
}
