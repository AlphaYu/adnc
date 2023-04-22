using Refit;
using Adnc.Shared.Rpc.Http.Rtos;

namespace Adnc.Shared.Rpc.Http.Services;

public interface IAuthRestClient : IRestClient
{
    /// <summary>
    ///  登录
    /// </summary>
    /// <returns></returns>
    [Post("/auth/api/session")]
    Task<ApiResponse<LoginRto>> LoginAsync(LoginInputRto loginRequest);

    /// <summary>
    ///  获取认证信息
    /// </summary>
    /// <returns></returns>
    [Get("/auth/api/session")]
    [Headers("Authorization: Basic", "Cache: 10000")]
    Task<ApiResponse<UserValidatedInfoRto>> GetValidatedInfoAsync();
    
    /// <summary>
    /// 获取当前用户权限
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 2000")]
    [Get("/auth/api/session/{userId}/permissions")]
    //Task<ApiResponse<List<string>>> GetCurrenUserPermissions([Header("Authorization")] string jwtToken, long userId, [Query(CollectionFormat.Multi)] string[] permissions);
    Task<ApiResponse<List<string>>> GetCurrenUserPermissionsAsync(long userId, [Query(CollectionFormat.Multi)] IEnumerable<string> requestPermissions, [Query]string userBelongsRoleIds);
}