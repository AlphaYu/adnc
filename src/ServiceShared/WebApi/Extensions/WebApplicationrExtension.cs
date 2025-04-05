using Adnc.Shared.WebApi.Registrar;

namespace Microsoft.AspNetCore.Builder;

public static class WebApplicationrExtension
{
    public static WebApplication UseAdnc(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app, $"{nameof(WebApplication)} is null.");

        var serviceInfo = app.Services.GetRequiredService<IServiceInfo>();
        var middlewareRegistrarType = serviceInfo.StartAssembly.ExportedTypes.Single(type => type.IsAssignableTo(typeof(AbstractWebApiMiddlewareRegistrar)) && type.IsNotAbstractClass(true));
        var middlewareRegistrar = (AbstractWebApiMiddlewareRegistrar?)Activator.CreateInstance(middlewareRegistrarType, app) ?? throw new InvalidOperationException($"Unable to create an instance of {middlewareRegistrarType.FullName}");
        middlewareRegistrar.UseAdnc();
        return app;
    }
}
