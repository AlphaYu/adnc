namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册跨域组件
    /// </summary>
    protected virtual void AddCors()
    {
        Services.AddCors(options =>
        {
            var _corsHosts = Configuration
            .GetValue("CorsHosts", string.Empty)
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            ;
            
            options.AddPolicy(ServiceInfo.CorsPolicy, policy =>
            {
                policy
                .WithOrigins(_corsHosts)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });
    }
}
