## ADNC 认证与授权
[GitHub 仓库地址](https://github.com/alphayu/adnc)

在 .NET Framework 时代，常用的是 Form 认证。对于前后端分离或多前端的系统，Form 认证的兼容性较差，容易导致代码复用率低、维护成本高。ASP.NET Core 对认证与授权进行了重构，采用基于声明（claims-based）的认证模型。

常见的认证组件包括：

- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Authentication.Cookies`
- `Microsoft.AspNetCore.Authentication.OpenIdConnect`
- `Microsoft.AspNetCore.Authentication.Auth`
- `Microsoft.AspNetCore.Authentication.Google`
- `Microsoft.AspNetCore.Authentication.Microsoft`
- `Microsoft.AspNetCore.Authentication.Facebook`
- `Microsoft.AspNetCore.Authentication.Twitter`

以上组件均由 .NET SDK 提供实现，统一基于 claims，并实现了 `Microsoft.AspNetCore.Authentication.Abstractions` 的相关抽象。在 ASP.NET Core 中，可以较容易地实现“混合认证”：同一个 Controller 内，不同 Action 可分别配置使用 Cookies 认证或 JwtBearer 认证等。相关示例可参考官方文档与社区实践。

为实现更灵活的认证策略（例如同一 Action 同时支持多种认证或指定认证方案），ADNC 引入了 Hybrid 与 Basic 两种认证方式。Hybrid 为项目自定义的路由型方案，Basic 为 HTTP 标准的基本认证。相关实现位于 Adnc.Shared.WebApi 工程的 Authentication 目录。

## 为什么要混合多种认证
在某些场景下，仅使用 JwtBearer 可能不足以满足需求。例如：

- 微服务间的同步调用：若服务 A 的某功能 X 会调用服务 B 的功能 Y，且仅采用 JwtBearer，则调用者必须同时具备 A: X 与 B: Y 的权限，导致权限传播复杂且管理成本高。

- 开放接口给第三方对接时，也可能遇到相同问题。

为应对微服务间的同步调用场景，Basic 认证可作为一种可选且更简单的鉴权方式，便于服务间互调。若业务场景不需此类支持，仍可仅使用JwtBearer。

为应对开发接口给对三分对接场景，可以再增加另外一种认证方式。


## 注册认证服务

```csharp
protected virtual void AddAuthentication<TAuthenticationHandler>() where TAuthenticationHandler : AbstractAuthenticationProcessor
{
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    var jwtSection = Configuration.GetRequiredSection(NodeConsts.JWT);
    var basicSection = Configuration.GetRequiredSection(NodeConsts.Basic);
    var jwtOptions = jwtSection.Get<JWTOptions>() ?? throw new InvalidDataException(nameof(jwtSection));
    Services
        .Configure<JWTOptions>(jwtSection)
        .Configure<BasicOptions>(basicSection)
        .AddScoped<AbstractAuthenticationProcessor, TAuthenticationHandler>()
        .AddAuthentication(HybridDefaults.AuthenticationScheme)
        .AddHybrid()
        .AddJwtBearer(options =>
        {
        })
        .AddBasic(options => options.Events.OnTokenValidated = (context) =>
        {
        });
}
```
- AddHybrid：注册 Hybrid 认证相关服务。

- AddBasic：注册 Basic 认证相关服务。

- AddJwtBearer：注册 JwtBearer 认证相关服务。  
  
  实现认证的核心为各具体的 AuthenticationHandler，上例中默认认证方案为 Hybrid；HybridAuthenticationHandler 仅负责路由（将请求转发到相应的认证处理器），不直接执行认证逻辑。
  
  - HybridAuthenticationHandler
  - JwtBearerAuthenticationHandler
  - BasicAuthenticationHandler

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

ADNC 在注册 Endpoint 中间件时，设置了全局认证拦截，即所有方法默认需要认证通过后才能访问。

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
- 需要`PermissionConsts.Dept.Create`权限，并且必须是JwtBearer认证方式。
```csharp
[Permission(PermissionConsts.Dept.Create)]
public async Task<ActionResult<long>> CreateAsync([FromBody] DeptCreationDto input)
```
- 需要`PermissionConsts.Dept.GetList`权限，同时支持JwtBearer与Basic两种认证方式
```csharp
[Permission(PermissionConsts.Dept.GetList, PermissionAttribute.JwtWithBasicSchemes)]
public async Task<ActionResult<List<DeptTreeDto>>> GetListAsync()
```

下面展示 AdncAuthorizeAttribute的实现，默认认证方案为 JwtBearer。

```csharp
public class AdncAuthorizeAttribute : AuthorizeAttribute
{
    public const string JwtWithBasicSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{Authentication.Basic.BasicDefaults.AuthenticationScheme}";

    public AdncAuthorizeAttribute(string code, string schemes = JwtBearerDefaults.AuthenticationScheme)
        : this([code], schemes)
    {
    }

    public AdncAuthorizeAttribute(string[] codes, string schemes = JwtBearerDefaults.AuthenticationScheme)
    {
        Codes = codes;
        Policy = AuthorizePolicy.Default;
        AuthenticationSchemes = schemes ?? throw new ArgumentNullException(nameof(schemes)); ;
    }

    public string[] Codes { get; set; }
}
```

## 客户端调用时如何指定认证方式

> JwtBearer => Authorization: Bearer {token}  
> Basic => Authorization: Basic {credentials}  
> 前端接口通常使用 JwtBearer；微服务之间的同步调用（除鉴权服务外）通常使用 Basic 认证以简化服务间鉴权。

- 要求服务端采用 JwtBearer 认证
```csharp
[Headers("Authorization: Bearer", "Cache: 2000")]
[Get("/usr/users/{userId}/permissions")]
Task<ApiResponse<List<string>>> GetCurrenUserPermissionsAsync(long userId, [Query(CollectionFormat.Multi)] IEnumerable<string> permissions, string validationVersion);
}
```

- 要求服务端采用 Basic 认证
```csharp
[Get("/usr/depts")]
[Headers("Authorization: Basic", "Cache: 2000")]
Task<ApiResponse<List<DeptRto>>> GeDeptsAsync();
```
—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
