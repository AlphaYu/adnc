using Adnc.Shared.Remote.Http;

namespace Adnc.Demo.Shared.Remote.Http.Services;

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