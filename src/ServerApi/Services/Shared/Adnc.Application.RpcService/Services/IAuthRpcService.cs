using Adnc.Application.RpcService.Rtos;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Application.RpcService.Services
{
    public interface IAuthRpcService : IRpcService
    {
        /// <summary>
        ///  登录
        /// </summary>
        /// <returns></returns>
        [Post("/usr/session")]
        Task<ApiResponse<LoginReplyRto>> LoginAsync(LoginRto loginRequest);

        /// <summary>
        /// 获取当前用户权限
        /// </summary>
        /// <returns></returns>
        [Headers("Authorization: Bearer", "Cache: 2000")]
        [Get("/usr/users/{userId}/permissions")]
        //Task<ApiResponse<List<string>>> GetCurrenUserPermissions([Header("Authorization")] string jwtToken, long userId, [Query(CollectionFormat.Multi)] string[] permissions);
        Task<ApiResponse<List<string>>> GetCurrenUserPermissionsAsync(long userId, [Query(CollectionFormat.Multi)] IEnumerable<string> permissions);
    }
}