using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Adnc.Application.Shared.RpcServices;
using Refit;

namespace Adnc.Cus.Application.RpcServices
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
        Task<ApiResponse<GetDictReply>> GetDict([Header("Authorization")] string jwtToken, long id);
    }
}