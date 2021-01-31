using System.Threading.Tasks;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Interceptors;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Services
{
    public interface IProductAppService : IAppService
    {
        [OpsLog(LogName = "创建商品")]
        Task<ProductDto> CreateAsync(ProductCreationDto inputDto);

        [OpsLog(LogName = "更新商品")]
        Task<ProductDto> UpdateAsync(ProductUpdationDto inputDto);

        [OpsLog(LogName = "调整商品价格")]
        Task<ProductDto> ChangePriceAsync(ProducChangePriceDto inputDto);

        [OpsLog(LogName = "上架商品")]
        Task<ProductDto> PutOnSale(ProductPutOnSaleDto input);

        [OpsLog(LogName = "下架商品")]
        Task<ProductDto> PutOffSale(ProductPutOffSaleDto input);

        Task<AppSrvResult<PageModelDto<ProductDto>>> GetPaged(ProductSearchDto search);
    }
}
