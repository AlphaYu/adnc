namespace Adnc.Demo.Ord.WebApi.Controllers;

/// <summary>
/// 订单管理
/// </summary>
[Route("ord/orders")]
[ApiController]
public class OrderController(IOrderService orderSrv) : AdncControllerBase
{
    /// <summary>
    /// 新建订单
    /// </summary>
    /// <param name="input"><see cref="OrderCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] OrderCreationDto input) => await orderSrv.CreateAsync(input);

    /// <summary>
    /// 订单付款
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
    /// 订单取消
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
    /// 订单删除
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
    /// 订单详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetAsync([FromRoute] long id) => await orderSrv.GetAsync(id);

    /// <summary>
    /// 订单分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<ActionResult<PageModelDto<OrderDto>>> GetPagedAsync([FromQuery] OrderSearchPagedDto input) => await orderSrv.GetPagedAsync(input);
}
