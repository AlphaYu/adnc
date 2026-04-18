using Adnc.Demo.Ord.Application.Contracts.Dtos.Order;

namespace Adnc.Demo.Ord.WebApi.Controllers;

/// <summary>
/// Order management
/// </summary>
[Route("ord/orders")]
[ApiController]
public class OrderController(IOrderService orderSrv) : AdncControllerBase
{
    /// <summary>
    /// Create an order
    /// </summary>
    /// <param name="input"><see cref="OrderCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] OrderCreationDto input) => await orderSrv.CreateAsync(input);

    /// <summary>
    /// Pay for an order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/payment")]
    public async Task<ActionResult> PayAsync([FromRoute] long id)
    {
        await orderSrv.PayAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Cancel an order
    /// </summary>
    /// <param name="id"></param>s
    /// <returns></returns>
    [HttpPut("{id}/status/canceler")]
    public async Task<ActionResult> CancelAsync([FromRoute] long id)
    {
        await orderSrv.CancelAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Delete an order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
    {
        await orderSrv.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Get order details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetAsync([FromRoute] long id) => await orderSrv.GetAsync(id);

    /// <summary>
    /// Get a paginated order list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<ActionResult<PageModelDto<OrderDto>>> GetPagedAsync([FromQuery] OrderSearchPagedDto input) => await orderSrv.GetPagedAsync(input);
}
