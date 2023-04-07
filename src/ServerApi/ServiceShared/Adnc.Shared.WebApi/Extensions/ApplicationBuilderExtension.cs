using Adnc.Shared.WebApi.Registrar;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationBuilderExtension
{
    /// <summary>
    /// 统一注册Adnc.WebApi通用中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAdnc(this IApplicationBuilder app)
    {
        var serviceInfo = app.ApplicationServices.GetRequiredService<IServiceInfo>();
        var middlewareRegistarType = serviceInfo.StartAssembly.ExportedTypes.FirstOrDefault(m => m.IsAssignableTo(typeof(IMiddlewareRegistrar)) && m.IsNotAbstractClass(true));
        if (middlewareRegistarType is null)
            throw new NullReferenceException(nameof(IMiddlewareRegistrar));

        if (Activator.CreateInstance(middlewareRegistarType, app) is not IMiddlewareRegistrar middlewareRegistar)
            throw new NullReferenceException(nameof(middlewareRegistar));

        middlewareRegistar.UseAdnc();

        return app;
    }
}