## 前言
&ensp;&ensp;&ensp;&ensp;我们在`NET Framework`时代，最常用的是Form认证。Form认证对于前后端分离或者多前端的系统来说不是太友好，很难相互兼容。很多都是一套后台管理系统，一套前端系统，一套API在物理层面分开，这样做代码复用率非常低，后期维护成本也高很多。 ASP.NET Core 对认证与授权进行了全新的设计，使用基于声明的认证(claims-based authentication)。
`Microsoft.AspNetCore.Authentication.JwtBearer`  
`Microsoft.AspNetCore.Authentication.Cookies`  
`Microsoft.AspNetCore.Authentication.OpenIdConnect`  
`Microsoft.AspNetCore.Authentication.Auth`  
`Microsoft.AspNetCore.Authentication.Google`  
`Microsoft.AspNetCore.Authentication.Microsoft`  
`Microsoft.AspNetCore.Authentication.FaceBook`  
`Microsoft.AspNetCore.Authentication.Twitter`  

以上这些都是.NET SDK已经实现了的，他们都是基于claims的，都是`Microsoft.AspNetCore.Authentication.Abstractions`的实现。在ASP.NET CORE时代，很容易做到混合认证，也就是同一个Controller里，可以一个Action支持Cookies认证，另外一个可以支持JwtBearer认证。网上很多这样案例，大家可以搜索看一下，很简单的配置就能实现。

然而，如果想实现的更灵活一些，比方目前ADNC的实现，同一个Action可以同时支持多种认证，也可以指定其中一种认证方式。为了实现这种方式，废了不少脑细胞。ADNC增加了Hybrid，Basic两种认证方式（Hybrid是自己取的名，Basic是http标准认证方式，相关代码在Adnc.Shared.WebApi工程的Authentication目录）。

## 为什么要混合两种认证
为什么呢？
目的是什么？
首先你可以只用JwtBeare认证，Basic认证是可选项目。我们在什么情况下可能需要使用Basic认证呢？
1、微服务之间同步通信，如果客户有A服务X功能的权限，但是X功能会调用B服务的Y功能。也就是如果采用JwtBeare认证，客户必须同时有A服务X功能和B服务Y功能权限才能正常使用。
2、开放接口给第三方合作方对接,也会遇到1同样的问题。当然，如果你的需求就是这样，你完全可以只采用JwtBearer认证。


## 注册认证服务

```csharp
public virtual void AddAuthentication()
{
   _services.AddAuthentication(HybridDefaults.AuthenticationScheme)
   .AddHybrid()
   .AddBasic()
   .AddJwtBearer(options => {xxxxx});
}
```
- AddHybrid 注册Hybrid认证相关服务
- AddBasic 注册Basic认证相关服务
- AddJwtBearer 注册JwtBearer认证相关服务。  
  
  实现认证的核心类是XxxAuthenticationHandler。
  通过上面的代码我们可以知道，系统默认的认证方式Hybrid，HybridAuthenticationHandler的主要作用就是路由，并不做实际的认证，系统所有认证请求都需要通过HybridAuthenticationHandler转发。

```csharp
public sealed class HybridAuthenticationHandler : AuthenticationHandler<HybridSchemeOptions>
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.IsNotNullOrWhiteSpace())
        {
            //jwtBearer
            if (authHeader.StartsWith(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                return await Context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            //Basic
            if (authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
                return await Context.AuthenticateAsync(BasicDefaults.AuthenticationScheme);
        }
    }
}
```
- BasicAuthenticationHandler
```csharp
public class BasicAuthenticationHandler : AuthenticationHandler<BasicSchemeOptions>
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        AuthenticateResult authResult;
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader != null && authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            var token = authHeader.Substring($"{BasicDefaults.AuthenticationScheme} ".Length).Trim();
            //校验token是否合法
            if (UnpackFromBase64(token, out string userName, out string appId))
            {
                var claims = new[] { new Claim("name", userName), new Claim(ClaimTypes.Role, "partner") };
                var identity = new ClaimsIdentity(claims, BasicDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);
                authResult = AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, BasicDefaults.AuthenticationScheme));

                var userContext = Context.RequestServices.GetService<IUserContext>();
                userContext.Id = appId.ToLong().Value;
                userContext.Account = userName;
                userContext.Name = userName;
                userContext.RemoteIpAddress = Context.Connection.RemoteIpAddress.MapToIPv4().ToString();

                return await Task.FromResult(authResult);
            }

            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            authResult = AuthenticateResult.Fail("Invalid Authorization Token");
            return await Task.FromResult(authResult);
        }
    }
}
```
## 注册授权服务
> 这里要注意，认证是认证，授权是授权。

```csharp
public virtual void AddAuthorization<THandler>() where THandler : AbstractPermissionHandler
{
    _services.AddScoped<IAuthorizationHandler, THandler>();
    _services.AddAuthorization(options =>
    {
      options.AddPolicy(AuthorizePolicy.Default, policy =>
      {
        policy.Requirements.Add(new PermissionRequirement());
      });
    });
}
```

- 授权服务核心代码在Adnc.Shared.WebApi工程的 AbstractPermissionHandler.cs 文件
```csharp
public abstract class AbstractPermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity.IsAuthenticated && context.Resource is HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            //Basic认证通过后并且Action允许Basic认证，那么默认有该功能的权限
            if (authHeader != null && authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            //JwtBearer认证通过后并且Action允许JwtBearer认证，还需要检查是否有该功能权限
            var userId = long.Parse(context.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
            var validationVersion = context.User.Claims.FirstOrDefault(x => x.Type == "version")?.Value;
            var codes = httpContext.GetEndpoint().Metadata.GetMetadata<PermissionAttribute>().Codes;
            var result = await CheckUserPermissions(userId, codes, validationVersion);
            if (result)
            {
                context.Succeed(requirement);
                return;
            }
        }
        context.Fail();
    }

    protected abstract Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes, string validationVersion);
}
```

## 设置Action特性

&ensp;&ensp;&ensp;&ensp;ADNC在注册Endpoint中间件时，设置了全局认证拦截，也就是所有方法默认需要认证通过后才能访问。

```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization();
});
```
- 允许匿名访问
```csharp
[AllowAnonymous]
public async Task<ActionResult<UserTokenInfoDto>> LoginAsync([FromBody] UserLoginDto input)
```
- 需要deptcreate权限，并且必须是JwtBearer认证方式。
```csharp
[Permission(PermissionConsts.Dept.Create)]
public async Task<ActionResult<long>> CreateAsync([FromBody] DeptCreationDto input)
```
- 需要deptlist权限，同时支持JwtBearer与Basic两种认证方式
```csharp
[Permission(PermissionConsts.Dept.GetList, PermissionAttribute.JwtWithBasicSchemes)]
public async Task<ActionResult<List<DeptTreeDto>>> GetListAsync()
```

我们来看下PermissionAttribute的代码，它派生自AuthorizeAttribute。默认认证方式是JwtBearer。

```csharp
public class PermissionAttribute : AuthorizeAttribute
{
    public const string JwtWithBasicSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{BasicDefaults.AuthenticationScheme}";

    public string[] Codes { get; set; }

    public PermissionAttribute(string code, string schemes = JwtBearerDefaults.AuthenticationScheme)
        : this(new string[] { code }, schemes)
    {
    }

    public PermissionAttribute(string[] codes, string schemes = JwtBearerDefaults.AuthenticationScheme)
    {
        Codes = codes;
        Policy = AuthorizePolicy.Default;
        if (schemes.IsNullOrEmpty())
            throw new ArgumentNullException(nameof(schemes));
        else
            AuthenticationSchemes = schemes;
    }
}

```

## 客户端调用时如何指定认证方式

> JwtBearer=>Authorization Bearer xxxxxxxxxxxxx  
> Basic=>Authorization Basic yyyyyyyyyyy
> 前端调用走的就是JwtBearer认证。
> 微服务间同步调用除了鉴权服务走的是JwtBearer认证，其它走的都是Basic认证

- 要求服务端采用JwtBearer认证
```csharp
[Headers("Authorization: Bearer", "Cache: 2000")]
[Get("/usr/users/{userId}/permissions")]
Task<ApiResponse<List<string>>> GetCurrenUserPermissionsAsync(long userId, [Query(CollectionFormat.Multi)] IEnumerable<string> permissions, string validationVersion);
}
```

- 要求服务端采用Basic认证
```csharp
[Get("/usr/depts")]
[Headers("Authorization: Basic", "Cache: 2000")]
Task<ApiResponse<List<DeptRto>>> GeDeptsAsync();
```
核心的业务代码在Adnc.Infra.Consul工程的SimpleDiscoveryDelegatingHandler.cs与ConsulDiscoverDelegatingHandler.cs文件。
```csharp
    public class SimpleDiscoveryDelegatingHandler : DelegatingHandler
    {
        public SimpleDiscoveryDelegatingHandler(IEnumerable<ITokenGenerator> tokenGenerators
            , IMemoryCache memoryCache)
        {
            _tokenGenerators = tokenGenerators;
            _memoryCache = memoryCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var auth = headers.Authorization;
            if (auth != null)
            {
                //这里
                var tokenGenerator = _tokenGenerators.FirstOrDefault(x => x.Scheme.EqualsIgnoreCase(auth.Scheme));
                //这里
                var tokenTxt = tokenGenerator?.Create();

                if (!string.IsNullOrEmpty(tokenTxt))
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }
        }
    }
```
-----------------------------
WELL DONE，记得 [star&&fork](https://github.com/alphayu/adnc)
全文完，[ADNC](https://aspdotnetcore.net)一个可以落地的.NET微服务/分布式开发框架。