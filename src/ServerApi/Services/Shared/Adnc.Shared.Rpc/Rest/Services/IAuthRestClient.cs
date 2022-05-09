namespace Adnc.Shared.Rpc.Rest.Services;

public interface IAuthRestClient : IRestClient
{
    /// <summary>
    ///  登录
    /// </summary>
    /// <returns></returns>
    [Post("/usr/session")]
    Task<ApiResponse<LoginRto>> LoginAsync(LoginInputRto loginRequest);
}