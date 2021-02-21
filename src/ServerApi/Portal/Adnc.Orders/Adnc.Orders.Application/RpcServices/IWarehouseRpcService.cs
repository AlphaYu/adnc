using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using Adnc.Application.Shared.RpcServices;
using Adnc.Orders.Application.RpcServices.Rtos;

namespace Adnc.Orders.Application.RpcServices
{
    public interface IWarehouseRpcService : IRpcService
    {
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        [Headers("Authorization: Bearer", "Cache: 1000")]
        [Get("/warehouse/products")]
        Task<ApiResponse<List<ProductRto>>> GetListAsync([Query(CollectionFormat.Multi)] string[] ids, CancellationToken cancellationToken = default);
    }
}