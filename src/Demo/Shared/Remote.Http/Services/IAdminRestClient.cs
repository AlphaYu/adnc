using Adnc.Shared.Remote.Http;

namespace Adnc.Demo.Remote.Http.Services;

public interface IAdminRestClient : IRestClient
{
    /// <summary>
    /// 获取字典选项
    /// </summary>
    /// <param name="codes"></param>
    /// <returns><see cref="List{DictOptionResponse}"/></returns>
    [Get("/api/admin/dicts/options")]
    //[Headers("Authorization: Basic", "Cache: 2000")]
    [Headers("Authorization: Basic")]
    Task<List<DictOptionResponse>> GetDictOptionsAsync(string? codes = null);

    /// <summary>
    /// 获取系统配置数据
    /// </summary>
    /// <param name="keys"></param>
    /// <returns><see cref="List{SysConfigSimpleResponse}"/></returns>
    [Get("/api/admin/sysconfigs")]
    [Headers("Authorization: Basic")]
    Task<List<SysConfigSimpleResponse>> GetSysConfigListAsync(string? keys = null);
}
