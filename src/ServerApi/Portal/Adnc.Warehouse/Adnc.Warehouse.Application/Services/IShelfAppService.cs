using System.Threading.Tasks;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Warehouse.Application.Dtos;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Warehouse.Application.Services
{
    public interface IShelfAppService: IAppService
    {
        [OpsLog(LogName = "创建货架")]
        Task<ShelfDto> CreateAsync(ShelfCreationDto input);

        [UnitOfWork(SharedToCap = true)]
        [OpsLog(LogName = "分配货架")]
        Task<ShelfDto> AllocateShelfToProductAsync(long shelfId, ShelfAllocateToProductDto input);

        Task<PageModelDto<ShelfDto>> GetPagedAsync(ShlefSearchDto search);

        [UnitOfWork]
        Task Test();
    }
}
