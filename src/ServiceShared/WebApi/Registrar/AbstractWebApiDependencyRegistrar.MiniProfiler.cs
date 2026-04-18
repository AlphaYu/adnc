namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Registers the MiniProfiler component.
    /// </summary>
    protected virtual void AddMiniProfiler()
    {
        var profilerPath = $"/{ServiceInfo.RelativeRootPath}/profiler";
        Services
            .AddMiniProfiler(options => options.RouteBasePath = profilerPath)
            .AddEntityFramework();
    }
}
