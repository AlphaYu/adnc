using Adnc.Shared.Rpc.Http;

namespace Adnc.Demo.Shared.Rpc.Http.Services;

public interface IMaintRestClient : IRestClient
{
    /// <summary>
    /// 获取字典数据
    /// </summary>
    /// <param name="jwtToken">token</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    [Get("/maint/api/dicts/{id}")]
    [Headers("Authorization: Basic", "Cache: 2000")]
    Task<ApiResponse<DictRto>> GetDictAsync(long id);
}