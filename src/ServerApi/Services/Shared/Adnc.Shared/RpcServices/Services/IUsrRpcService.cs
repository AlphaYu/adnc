namespace Adnc.Shared.RpcServices.Services;

public interface IUsrRpcService : IRpcService
{
    /// <summary>
    /// 获取部门列表
    /// </summary>
    /// <param name="jwtToken">token</param>
    /// <param name="id">id</param>
    /// <returns></returns>
    [Get("/usr/depts")]
    [Headers("Authorization: Basic", "Cache: 2000")]
    Task<List<DeptRto>> GeDeptsAsync();
}