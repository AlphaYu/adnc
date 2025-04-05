using System.Reflection;

namespace Adnc.Shared;

public interface IServiceInfo
{
    /// <summary>
    /// The ID associated with the service.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// The name associated with the service.
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// The cross-origin resource sharing (CORS) policy associated with the service.
    /// </summary>
    public string CorsPolicy { get; set; }

    /// <summary>
    /// The short name associated with the service.
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// The relative root path associated with the service.
    /// </summary>
    public string RelativeRootPath { get; }

    /// <summary>
    /// The version associated with the service.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// The description associated with the service.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The assembly associated with the service startup.
    /// </summary>
    public Assembly StartAssembly { get; }

    /// <summary>
    /// Migrations Assembly Name
    /// </summary>
    public string MigrationsAssemblyName { get; }
}

public sealed class ServiceInfo : IServiceInfo
{
    private static ServiceInfo? _instance;
    private static readonly object _lockObj = new();

    private ServiceInfo()
    {
    }

    public string Id { get; private set; } = string.Empty;
    public string ServiceName { get; private set; } = string.Empty;
    public string CorsPolicy { get; set; } = string.Empty;
    public string ShortName { get; private set; } = string.Empty;
    public string RelativeRootPath { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Assembly StartAssembly { get; private set; } = default!;
    public string MigrationsAssemblyName { get; private set; } = string.Empty;

    public static ServiceInfo CreateInstance(Assembly startAssembly, string? migrationsAssemblyName = null)
    {
        if (_instance is not null)
        {
            return _instance;
        }

        lock (_lockObj)
        {
            if (_instance is not null)
            {
                return _instance;
            }

            startAssembly ??= Assembly.GetEntryAssembly() ?? throw new InvalidOperationException(nameof(startAssembly));

            var attribute = startAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var description = attribute is null ? string.Empty : attribute.Description;
            var version = startAssembly.GetName().Version ?? throw new InvalidOperationException("startAssembly.GetName().Version");
            var startAssemblyName = startAssembly.GetName().Name ?? string.Empty;
            var serviceName = startAssemblyName.Replace(".", "-").ToLower();
            var ticks = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            var ticksHex = Convert.ToString(ticks, 16);
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
            var serviceId = envName switch
            {
                "development" => $"{serviceName}-dev-{ticksHex}",
                "test" => $"{serviceName}-test-{ticksHex}",
                "staging" => $"{serviceName}-stag-{ticksHex}",
                "production" => $"{serviceName}-{ticksHex}",
                _ => throw new InvalidOperationException("ASPNETCORE_ENVIRONMENT")
            };

            var names = startAssemblyName.Split(".");
            migrationsAssemblyName ??= startAssemblyName.Replace($".{names.Last()}", ".Migrations");
            _instance = new ServiceInfo
            {
                Id = serviceId,
                ServiceName = serviceName,
                ShortName = $"{names[^2]}-{names[^1]}".ToLower(),
                RelativeRootPath = $"{names[^1]}/{names[^2]}".ToLower(),
                CorsPolicy = "default",
                StartAssembly = startAssembly,
                Description = description,
                Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}",
                MigrationsAssemblyName = migrationsAssemblyName
            };
        }
        return _instance;
    }

    public static ServiceInfo GetInstance()
        => _instance ?? throw new InvalidOperationException(nameof(ServiceInfo));
}
