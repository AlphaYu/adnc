namespace Adnc.Shared.Rpc.Rest.Services;

public interface IAuthRestClient : IRestClient
{
    /// <summary>
    ///  登录
    /// </summary>
    /// <returns></returns>
    [Post("/auth/session")]
    Task<ApiResponse<LoginRto>> LoginAsync(LoginInputRto loginRequest);

    /// <summary>
    ///  获取认证信息
    /// </summary>
    /// <returns></returns>
    [Get("/auth/session")]
    [Headers("Authorization: Bearer")]
    Task<ApiResponse<UserValidatedInfoRto>> GetValidatedInfoAsync();
}