using Adnc.Shared.WebApi.Authentication.Basic;
using Adnc.Shared.WebApi.Authentication.Bearer;
using Adnc.Shared.WebApi.Authentication.Hybrid;
using Adnc.Shared.WebApi.Authentication.Processors;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册身份认证组件
    /// </summary>
    /// <typeparam name="TAuthenticationHandler"></typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    protected virtual void AddAuthentication<TAuthenticationHandler>() where TAuthenticationHandler : AbstractAuthenticationProcessor
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var jwtSection = Configuration.GetRequiredSection(NodeConsts.JWT);
        var basicSection = Configuration.GetRequiredSection(NodeConsts.Basic);
        var jwtConfig = jwtSection.Get<JWTOptions>() ?? throw new InvalidDataException(nameof(jwtSection));
        Services
            .Configure<JWTOptions>(jwtSection)
            .Configure<BasicOptions>(basicSection)
            .AddScoped<AbstractAuthenticationProcessor, TAuthenticationHandler>()
            .AddAuthentication(HybridDefaults.AuthenticationScheme)
            .AddHybrid()
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = jwtConfig.GenarateTokenValidationParameters();
                options.Events = new JwtBearerEvents
                {
                    //接受到消息时调用
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    },
                    //未授权时调用
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    },
                    //认证失败时调用
                    OnAuthenticationFailed = context =>
                    {
                        //如果是过期，在http heard中加入act参数
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.TryAdd("act", "expired");
                        }

                        return Task.CompletedTask;
                    },
                    //在Token验证通过后调用
                    OnTokenValidated = async (context) =>
                    {
                        var principal = context.Principal ?? throw new ArgumentNullException(nameof(context.Principal));
                        var authenticationProcessor = context.HttpContext.RequestServices.GetRequiredService<AbstractAuthenticationProcessor>();
                        var claims = await authenticationProcessor.ValidateAsync(context.Principal);
                        if (claims.IsNullOrEmpty())
                        {
                            var message = "invalid authorization token,claims is null";
                            context.Fail(message);
                            context.Response.Headers.TryAdd("act", message);
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            var userContext = context.HttpContext.RequestServices.GetRequiredService<UserContext>();
                            userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
                            userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
                            userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
                            userContext.RoleIds = claims.First(x => x.Type == "roleids").Value;
                            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
                            userContext.RemoteIpAddress = remoteIpAddress is null ? string.Empty : remoteIpAddress.MapToIPv4().ToString();
                        }
                    }
                };
            })
            .AddBasic(options => options.Events.OnTokenValidated = (context) =>
            {
                var userContext = context.HttpContext.RequestServices.GetRequiredService<UserContext>();
                var principal = context.Principal ?? throw new ArgumentNullException(nameof(context.Principal));
                var claims = principal.Claims;
                userContext.Id = long.Parse(claims.First(x => x.Type == BasicDefaults.NameId).Value);
                userContext.Account = claims.First(x => x.Type == BasicDefaults.UniqueName).Value;
                userContext.Name = claims.First(x => x.Type == BasicDefaults.Name).Value;
                var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;
                userContext.RemoteIpAddress = remoteIpAddress is null ? string.Empty : remoteIpAddress.MapToIPv4().ToString();
                return Task.CompletedTask;
            });
    }
}
