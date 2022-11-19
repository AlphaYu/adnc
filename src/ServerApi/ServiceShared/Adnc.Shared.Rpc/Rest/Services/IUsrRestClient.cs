namespace Adnc.Shared.Rpc.Rest.Services;

public interface IUsrRestClient : IRestClient
{
    /// <summary>
    /// 获取当前用户权限
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 2000")]
    [Get("/usr/users/{userId}/permissions")]
    //Task<ApiResponse<List<string>>> GetCurrenUserPermissions([Header("Authorization")] string jwtToken, long userId, [Query(CollectionFormat.Multi)] string[] permissions);
    Task<ApiResponse<List<string>>> GetCurrenUserPermissionsAsync(long userId, [Query(CollectionFormat.Multi)] IEnumerable<string> requestPermissions, [Query]string userBelongsRoleIds);

    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="jwtToken">token</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    [Get("/usr/depts")]
    [Headers("Authorization: Basic", "Cache: 2000")]
    Task<ApiResponse<List<DeptRto>>> GeDeptsAsync();
}