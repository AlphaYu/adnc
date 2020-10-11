using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace Adnc.Core.Shared.RpcServices
{
    public interface IAuthRpcService : IRpcService
    {
        /// <summary>
        ///  登录
        /// </summary>
        /// <returns></returns>
        [Post("/usr/session")]
        Task<LoginReply> Login(LoginRequest loginRequest);

        /// <summary>
        /// 获取当前用户权限
        /// </summary>
        /// <returns></returns>
        [Get("/usr/users/{userId}/permissions")]
        Task<IEnumerable<string>> GetCurrenUserPermissions([Header("Authorization")] string jwtToken,long userId, string[] permissions);
    }
}
