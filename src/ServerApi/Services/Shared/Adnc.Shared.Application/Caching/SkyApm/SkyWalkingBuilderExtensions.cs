namespace Adnc.Shared.Application.Caching.SkyApm;

public static class SkyWalkingBuilderExtensions
{
    public static SkyApmExtensions AddCaching(this SkyApmExtensions extensions)
    {
        if (extensions is not null)
            extensions.Services.AddSingleton<ITracingDiagnosticProcessor, CacheTracingDiagnosticProcessor>();

        return extensions;
    }
}