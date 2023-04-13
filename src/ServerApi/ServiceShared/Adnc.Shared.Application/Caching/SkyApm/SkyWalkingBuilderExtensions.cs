namespace Adnc.Shared.Application.Caching.SkyApm;

public static class SkyWalkingBuilderExtensions
{
    public static SkyApmExtensions AddRedisCaching(this SkyApmExtensions extensions)
    {
        //if (extensions is not null)
            extensions.Services.AddSingleton<ITracingDiagnosticProcessor, CacheTracingDiagnosticProcessor>();

        return extensions;
    }
}