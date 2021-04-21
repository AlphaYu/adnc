using System.Threading.Tasks;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Interceptors;
using Adnc.Whse.Application.Contracts.Dtos;
using Adnc.Application.Shared.Dtos;
using System.Collections.Generic;

namespace Adnc.Whse.Application.Contracts.Services
{
    public interface IProductAppService : IAppService
    {
        [OpsLog(LogName = "创建商品")]
        Task<ProductDto> CreateAsync(ProductCreationDto input);

        [OpsLog(LogName = "更新商品")]
        Task<ProductDto> UpdateAsync(long id, ProductUpdationDto input);

        [OpsLog(LogName = "调整商品价格")]
        Task<ProductDto> ChangePriceAsync(long id, ProducChangePriceDto input);

        [OpsLog(LogName = "上架商品")]
        Task<ProductDto> PutOnSaleAsync(long id, ProductPutOnSaleDto input);

        [OpsLog(LogName = "下架商品")]
        Task<ProductDto> PutOffSaleAsync(long id, ProductPutOffSaleDto input);

        Task<PageModelDto<ProductDto>> GetPagedAsync(ProductSearchPagedDto search);

        Task<List<ProductDto>> GetListAsync(ProductSearchListDto search);

    }
}
