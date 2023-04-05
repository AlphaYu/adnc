namespace Adnc.Shared.WebApi;

public class ServiceInfo : IServiceInfo
{
    private static ServiceInfo? _instance = null;
    private static readonly object _lockObj = new();

    public string Id { get; private set; } = string.Empty;
    public string ServiceName { get; private set; } = string.Empty;
    public string CorsPolicy { get; set; } = string.Empty;
    public string ShortName { get; private set; } = string.Empty;
    public string RelativeRootPath { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Assembly StartAssembly { get; private set; } = default!;
    public string MigrationsAssemblyName { get; private set; } = string.Empty;

    private ServiceInfo()
    {
    }

    public static ServiceInfo CreateInstance(Assembly startAssembly, string? migrationsAssemblyName = null)
    {
        if (_instance is not null)
            return _instance;

        lock (_lockObj)
        {
            if (_instance is not null)
                return _instance;

            if (startAssembly is null)
                startAssembly = Assembly.GetEntryAssembly() ?? throw new NullReferenceException(nameof(startAssembly));

            var attribute = startAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
            var description = attribute is null ? string.Empty : attribute.Description;
            var version = startAssembly.GetName().Version ?? throw new NullReferenceException("startAssembly.GetName().Version");
            var startAssemblyName = startAssembly.GetName().Name ?? string.Empty;
            var serviceName = startAssemblyName.Replace(".", "-").ToLower();
            var ticks = DateTime.Now.GetTotalMilliseconds().ToLong();
            var ticksHex = Convert.ToString(ticks, 16);
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
            var serviceId = envName switch
            {
                "development" => $"{serviceName}-dev-{ticksHex}",
                "test" => $"{serviceName}-test-{ticksHex}",
                "staging" => $"{serviceName}-stag-{ticksHex}",
                "production" => $"{serviceName}-{ticksHex}",
                _ => throw new NullReferenceException("ASPNETCORE_ENVIRONMENT")
            };

            var names = startAssemblyName.Split(".");
            migrationsAssemblyName ??= startAssemblyName.Replace($".{names.Last()}", ".Migrations");
            _instance = new ServiceInfo
            {
                Id = serviceId,
                ServiceName = serviceName,
                ShortName = $"{names[^2]}-{names[^1]}".ToLower(),
                RelativeRootPath = $"{names[^2]}/{names[^1]}".ToLower(),
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
    {
        if (_instance is null)
            throw new NullReferenceException(nameof(ServiceInfo));

        return _instance;
    }
}