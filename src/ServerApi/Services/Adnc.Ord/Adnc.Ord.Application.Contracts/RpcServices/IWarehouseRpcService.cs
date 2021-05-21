using Adnc.Application.Shared.RpcServices;
using Adnc.Ord.Application.Contracts.RpcServices.Rtos;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Ord.Application.Contracts.RpcServices
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