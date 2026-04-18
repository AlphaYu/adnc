using Adnc.Demo.Whse.Application.Contracts.Dtos.Product;

namespace Adnc.Demo.Whse.Api.Controllers;

/// <summary>
/// Product management
/// </summary>
[Route("whse/products")]
[ApiController]
public class ProductController(IProductService productSrv) : AdncControllerBase
{
    /// <summary>
    /// Create a product
    /// </summary>
    /// <param name="input"><see cref="ProductCreationDto"/></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateAsync([FromBody] ProductCreationDto input) => await productSrv.CreateAsync(input);

    /// <summary>
    /// Update a product
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateAsync([FromRoute] long id, [FromBody] ProductUpdationDto input) => await productSrv.UpdateAsync(id, input);

    /// <summary>
    /// Change the price
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/price")]
    public async Task<ActionResult<ProductDto>> ChangePriceAsync([FromRoute] long id, ProducChangePriceDto input) => await productSrv.ChangePriceAsync(id, input);

    /// <summary>
    /// Put a product on sale
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/status/1001")]
    public async Task<ActionResult<ProductDto>> PutOnSaleAsync([FromRoute] long id, ProductPutOnSaleDto input) => await productSrv.PutOnSaleAsync(id, input);

    /// <summary>
    /// Take a product off sale
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/status/1002")]
    public async Task<ActionResult<ProductDto>> PutOffSaleAsync([FromRoute] long id, ProductPutOffSaleDto input) => await productSrv.PutOffSaleAsync(id, input);

    /// <summary>
    /// Get a product list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetListAsync([FromQuery] ProductSearchListDto input) => await productSrv.GetListAsync(input);

    /// <summary>
    /// Get a paginated product list
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public async Task<ActionResult<PageModelDto<ProductDto>>> GetPagedAsync([FromQuery] ProductSearchPagedDto input) => await productSrv.GetPagedAsync(input);
}
