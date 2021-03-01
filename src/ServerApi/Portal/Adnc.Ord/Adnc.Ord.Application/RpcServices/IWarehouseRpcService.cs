using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Adnc.Application.Shared.RpcServices;
using Adnc.Ord.Application.RpcServices.Rtos;

namespace Adnc.Ord.Application.RpcServices
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