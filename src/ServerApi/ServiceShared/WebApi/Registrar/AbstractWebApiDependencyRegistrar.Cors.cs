using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册跨域组件
    /// </summary>
    protected virtual void AddCors()
    {
        var corsHosts = Configuration.GetValue("CorsHosts", string.Empty);
        Action<CorsPolicyBuilder> corsPolicyAction = (corsPolicy) => corsPolicy.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        if (corsHosts == "*")
            corsPolicyAction += (corsPolicy) => corsPolicy.SetIsOriginAllowed(_ => true);
        else
            corsPolicyAction += (corsPolicy) => corsPolicy.WithOrigins(corsHosts.Split(','));

        Services.AddCors(options => options.AddPolicy(ServiceInfo.CorsPolicy, corsPolicyAction));
    }
}
