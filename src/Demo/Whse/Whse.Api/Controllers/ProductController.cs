namespace Adnc.Demo.Whse.WebApi.Controllers;

/// <summary>
/// 商品管理
/// </summary>
[Route("whse/products")]
[ApiController]
public class ProductController(IProductService productSrv) : AdncControllerBase
{
    /// <summary>
    /// 新建商品
    /// </summary>
    /// <param name="input"><see cref="ProductCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] ProductCreationDto input) => await productSrv.CreateAsync(input);

    /// <summary>
    /// 更新商品
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateAsync([FromRoute] long id, [FromBody] ProductUpdationDto input) => await productSrv.UpdateAsync(id, input);

    /// <summary>
    /// 调整价格
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/price")]
    public async Task<ActionResult<ProductDto>> ChangePriceAsync([FromRoute] long id, ProducChangePriceDto input) => await productSrv.ChangePriceAsync(id, input);

    /// <summary>
    /// 上架商品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/status/1001")]
    public async Task<ActionResult<ProductDto>> PutOnSaleAsync([FromRoute] long id, ProductPutOnSaleDto input) => await productSrv.PutOnSaleAsync(id, input);

    /// <summary>
    /// 下架商品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/status/1002")]
    public async Task<ActionResult<ProductDto>> PutOffSaleAsync([FromRoute] long id, ProductPutOffSaleDto input) => await productSrv.PutOffSaleAsync(id, input);

    /// <summary>
    /// 商品列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetListAsync([FromQuery] ProductSearchListDto input) => await productSrv.GetListAsync(input);

    /// <summary>
    /// 商品分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<ActionResult<PageModelDto<ProductDto>>> GetPagedAsync([FromQuery] ProductSearchPagedDto input) => await productSrv.GetPagedAsync(input);
}
