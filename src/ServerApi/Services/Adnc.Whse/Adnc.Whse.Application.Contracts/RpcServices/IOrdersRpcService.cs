﻿using Adnc.Application.Shared.RpcServices;

namespace Adnc.Whse.Application.Contracts.RpcServices
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