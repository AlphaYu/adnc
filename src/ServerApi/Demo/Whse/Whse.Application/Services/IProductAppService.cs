namespace Adnc.Demo.Whse.Application.Services;

public interface IProductAppService : IAppService
{
    [OperateLog(LogName = "创建商品")]
    Task<ProductDto> CreateAsync(ProductCreationDto input);

    [OperateLog(LogName = "更新商品")]
    Task<ProductDto> UpdateAsync(long id, ProductUpdationDto input);

    [OperateLog(LogName = "调整商品价格")]
    Task<ProductDto> ChangePriceAsync(long id, ProducChangePriceDto input);

    [OperateLog(LogName = "上架商品")]
    Task<ProductDto> PutOnSaleAsync(long id, ProductPutOnSaleDto input);

    [OperateLog(LogName = "下架商品")]
    Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input);

    Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchPagedDto search);

    Task<List<ProductDto>> GetListAsync(ProductSearchListDto search);
}