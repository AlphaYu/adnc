## 前言
&ensp;&ensp;&ensp;&ensp;在 .NET Framework 时代，常用的是 Form 认证。对于前后端分离或多前端的系统，Form 认证的兼容性较差，导致代码复用率低且维护成本高。ASP.NET Core 对认证与授权进行了重构，采用基于声明（claims-based）的认证模型。
`Microsoft.AspNetCore.Authentication.JwtBearer`  
`Microsoft.AspNetCore.Authentication.Cookies`  
`Microsoft.AspNetCore.Authentication.OpenIdConnect`  
`Microsoft.AspNetCore.Authentication.Auth`  
`Microsoft.AspNetCore.Authentication.Google`  
`Microsoft.AspNetCore.Authentication.Microsoft`  
`Microsoft.AspNetCore.Authentication.Facebook`  
`Microsoft.AspNetCore.Authentication.Twitter`  

以上这些都是.NET SDK已经实现了的，他们都是基于claims的，都是`Microsoft.AspNetCore.Authentication.Abstractions`的实现。在ASP.NET CORE时代，很容易做到混合认证，也就是同一个Controller里，可以一个Action支持Cookies认证，另外一个可以支持JwtBearer认证。网上很多这样案例，大家可以搜索看一下，很简单的配置就能实现。

为实现更灵活的认证策略（例如同一 Action 同时支持多种认证或指定认证方案），ADNC 引入了 Hybrid 与 Basic 两种认证方式。Hybrid 为项目自定义的路由型方案，Basic 为 HTTP 标准的基本认证。相关实现位于 Adnc.Shared.WebApi 工程的 Authentication 目录。

## 为什么要混合两种认证
在某些场景下，仅使用 JwtBearer 可能不足以满足需求。例如：

- 微服务间的同步调用：若服务 A 的某功能 X 会调用服务 B 的功能 Y，且仅采用 JwtBearer，则调用者必须同时具备 A: X 与 B: Y 的权限，导致权限传播复杂且管理成本高。

- 开放接口给第三方对接时，也可能遇到相同问题。

为应对上述场景，Basic 认证可作为一种可选且更简单的鉴权方式，便于服务间互调。若业务场景不需此类支持，仍可仅使用 JwtBearer。


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
  
  实现认证的核心为各具体的 AuthenticationHandler。上例中默认认证方案为 Hybrid；HybridAuthenticationHandler 仅负责路由（将请求转发到相应的认证处理器），不直接执行认证逻辑。

```csharp
public sealed class HybridAuthenticationHandler : AuthenticationHandler<HybridSchemeOptions>
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.IsNotNullOrWhiteSpace())
        {
            // JwtBearer 认证
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
            // 校验 token 是否合法
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
> 注意：认证（Authentication）与授权（Authorization）为不同概念。

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
            // 若使用 Basic 认证，且 Action 允许 Basic，则默认通过权限检查。
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

下面展示 PermissionAttribute 的实现（派生自 AuthorizeAttribute）。默认认证方案为 JwtBearer。

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

> JwtBearer => Authorization: Bearer {token}  
> Basic => Authorization: Basic {credentials}  
> 前端接口通常使用 JwtBearer；微服务之间的同步调用（除鉴权服务外）通常使用 Basic 认证以简化服务间鉴权。

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
                // 使用 TokenGenerator 生成 token 文本
                var tokenTxt = tokenGenerator?.Create();

                if (!string.IsNullOrEmpty(tokenTxt))
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }
        }
    }
```
-----------------------------

WELL DONE，记得 [star & fork](https://github.com/alphayu/adnc)

全文完，[ADNC](https://aspdotnetcore.net) —— 一个可以落地的 .NET 微服务/分布式开发框架。