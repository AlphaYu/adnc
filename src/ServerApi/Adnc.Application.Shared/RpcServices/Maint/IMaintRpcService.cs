using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;
using Adnc.Application.Shared.RpcServices.Rtos;

namespace Adnc.Application.Shared.RpcServices
{
    public interface IMaintRpcService : IRpcService
    {
        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="jwtToken">token</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        [Get("/maint/dicts/{id}")]
        [Headers("Authorization: Bearer")]
        Task<ApiResponse<DictRto>> GetDictAsync(long id);
    }
}
