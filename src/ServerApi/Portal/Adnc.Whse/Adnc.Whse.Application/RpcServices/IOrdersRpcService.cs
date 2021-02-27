using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Shared.RpcServices;
using Refit;

namespace Adnc.Whse.Application.RpcServices
{
    public interface IOrdersRpcService : IRpcService
    {
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="jwtToken">token</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        //[Get("/orders/{id}")]
        //[Headers("Authorization: Bearer")]
        //Task<ApiResponse<DictRto>> GetOrderAsync(long id);
    }
}