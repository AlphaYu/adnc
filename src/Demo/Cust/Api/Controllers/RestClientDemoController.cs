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
    /// Get the dictionary option list
    /// </summary>
    /// <param name="codes"></param>
    /// <returns></returns>
    [AllowAnonymous, HttpGet()]
    [Route("dictoptions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DictOptionResponse>>> GetDictOptionsAsync(string codes = "all")
    {
        var messages = await adminRestClient.GetDictOptionsAsync(codes);
        return messages ?? [];
    }

    /// <summary>
    /// Get the system configuration list
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    [AllowAnonymous, HttpGet()]
    [Route("sysconfigs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SysConfigSimpleResponse>>> GetSysConfigListAsync(string keys = "all")
    {
        var messages = await adminRestClient.GetSysConfigListAsync(keys);
        return messages ?? [];
    }
}
