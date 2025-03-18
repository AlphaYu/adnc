using Adnc.Demo.Remote.Http.Messages;
using Adnc.Demo.Remote.Http.Services;

namespace Adnc.Demo.Cust.Api.Controllers;

/// <summary>
/// REST  demo
/// </summary>
[Route($"{RouteConsts.CustRoot}/rest")]
[ApiController]
public class RestClientDemoController(IAdminRestClient adminRestClient) : AdncControllerBase
{
    /// <summary>
    /// 获取字典数据选项列表
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    [AllowAnonymous,HttpGet()]
    [Route("dictoptions")]
    public async Task<ActionResult<List<DictOptionResponse>>> GetDictOptionsAsync(string codes = "all")
    {
        var restResult = await adminRestClient.GetDictOptionsAsync(codes);
        if (restResult.IsSuccessStatusCode && restResult.Content is not null)
            return restResult.Content;
        return Problem(restResult.Error, restResult.Error.StatusCode);
    }

    /// <summary>
    /// 获取系统配置列表
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    [AllowAnonymous, HttpGet()]
    [Route("sysconfigs")]
    public async Task<ActionResult<List<SysConfigSimpleResponse>>> GetSysConfigListAsync(string keys = "all")
    {
        var restResult = await adminRestClient.GetSysConfigListAsync(keys);
        if (restResult.IsSuccessStatusCode && restResult.Content is not null)
            return restResult.Content;
        return Problem(restResult.Error, restResult.Error.StatusCode);
    }
}