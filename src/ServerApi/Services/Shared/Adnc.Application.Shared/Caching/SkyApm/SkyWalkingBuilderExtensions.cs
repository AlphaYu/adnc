using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using SkyApm.Utilities.DependencyInjection;
using System;

namespace Adnc.Application.Shared.Caching
{
    public static class SkyWalkingBuilderExtensions
    {
        public static SkyApmExtensions AddCaching(this SkyApmExtensions extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            extensions.Services.AddSingleton<ITracingDiagnosticProcessor, CacheTracingDiagnosticProcessor>();

            return extensions;
        }
    }
}