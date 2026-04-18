using Adnc.Demo.Whse.Application.Contracts.Dtos.Product;

namespace Adnc.Demo.Whse.Application.Contracts.Interfaces;

public interface IProductService : IAppService
{
    [OperateLog(LogName = "Create product")]
    Task<ProductDto> CreateAsync(ProductCreationDto input);

    [OperateLog(LogName = "Update product")]
    Task<ProductDto> UpdateAsync(long id, ProductUpdationDto input);

    [OperateLog(LogName = "Change product price")]
    Task<ProductDto> ChangePriceAsync(long id, ProducChangePriceDto input);

    [OperateLog(LogName = "Put product on sale")]
    Task<ProductDto> PutOnSaleAsync(long id, ProductPutOnSaleDto input);

    [OperateLog(LogName = "Take product off sale")]
    Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input);

    Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchPagedDto input);

    Task<List<ProductDto>> GetListAsync(ProductSearchListDto input);
}
