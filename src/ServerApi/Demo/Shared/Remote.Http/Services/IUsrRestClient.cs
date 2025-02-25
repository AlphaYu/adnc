using Adnc.Shared.Remote.Http;

namespace Adnc.Demo.Shared.Remote.Http.Services;

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