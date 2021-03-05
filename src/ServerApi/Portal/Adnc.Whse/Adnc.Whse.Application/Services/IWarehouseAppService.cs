using System.Threading.Tasks;
using Adnc.Application.Shared.Dtos;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Whse.Application.Dtos;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Whse.Application.Services
{
    /// <summary>
    /// 仓储管理
    /// </summary>
    public interface IWarehouseAppService: IAppService
    {
        /// <summary>
        /// 创建仓储
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OpsLog(LogName = "创建仓储")]
        Task<WarehouseDto> CreateAsync(WarehouseCreationDto input);

        /// <summary>
        /// 分配仓储给商品
        /// </summary>
        /// <param name="shelfId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(SharedToCap = true)]
        [OpsLog(LogName = "分配货架")]
        Task<WarehouseDto> AllocateShelfToProductAsync(long shelfId, WarehouseAllocateToProductDto input);

        /// <summary>
        /// 锁定商品库存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        [OpsLog(LogName = "锁定库存")]
        Task BlockQtyAsync(WarehouseBlockQtyDto input);

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<PageModelDto<WarehouseDto>> GetPagedAsync(WarehouseSearchDto search);
    }
}
