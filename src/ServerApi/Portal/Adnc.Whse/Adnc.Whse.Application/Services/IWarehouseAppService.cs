using System.Threading.Tasks;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Whse.Application.Dtos;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Whse.Application.Services
{
    public interface IWarehouseAppService: IAppService
    {
        [OpsLog(LogName = "创建货架")]
        Task<WarehouseDto> CreateAsync(WarehouseCreationDto input);

        [UnitOfWork(SharedToCap = true)]
        [OpsLog(LogName = "分配货架")]
        Task<WarehouseDto> AllocateShelfToProductAsync(long shelfId, WarehouseAllocateToProductDto input);

        [UnitOfWork]
        [OpsLog(LogName = "锁定库存")]
        Task BlockQtyAsync(WarehouseBlockQtyDto input);

        Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search);
    }
}
