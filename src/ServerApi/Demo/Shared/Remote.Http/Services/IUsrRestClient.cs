using Adnc.Shared.Rpc.Http;

namespace Adnc.Demo.Shared.Rpc.Http.Services;

public interface IUsrRestClient : IRestClient
{
    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <returns></returns>
    [Get("/usr/api/organizations")]
    [Headers("Authorization: Basic", "Cache: 2000")]
    Task<ApiResponse<List<DeptRto>>> GetOrganizationsAsync();
}