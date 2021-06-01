using Adnc.Application.RpcService.Rtos;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Application.RpcService.Services
{
    public interface IWhseRpcService : IRpcService
    {
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        [Headers("Authorization: Bearer", "Cache: 1000")]
        [Get("/whse/products")]
        Task<ApiResponse<List<ProductRto>>> GetProductsAsync(ProductSearchListRto search, CancellationToken cancellationToken = default);
    }
}