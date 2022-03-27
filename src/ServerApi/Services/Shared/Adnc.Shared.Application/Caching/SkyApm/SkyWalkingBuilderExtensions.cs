namespace Adnc.Shared.Application.Caching.SkyApm;

public static class SkyWalkingBuilderExtensions
{
    public static SkyApmExtensions AddCaching(this SkyApmExtensions extensions)
    {
        if (extensions == null)
            throw new ArgumentNullException(nameof(extensions));

        extensions.Services.AddSingleton<ITracingDiagnosticProcessor, CacheTracingDiagnosticProcessor>();

        return extensions;
    }
}