namespace Adnc.Shared.WebApi;

public class ServiceInfo : IServiceInfo
{
    public string Id { get; private set; }
    public string ServiceName { get; private set; }
    public string CorsPolicy { get; set; }
    public string ShortName { get; private set; }
    public string Version { get; private set; }
    public string Description { get; private set; }
    public Assembly StartAssembly { get; private set; }

    public ServiceInfo(Assembly startAssembly, IHostEnvironment env)
    {
        if (startAssembly is null)
            startAssembly = Assembly.GetEntryAssembly();

        var description = startAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        var assemblyName = startAssembly.GetName();
        var version = assemblyName.Version;
        var fullName = assemblyName.Name.ToLower();
        var serviceName = fullName.Replace(".", "-");
        if (env.IsDevelopment())
            serviceName += "-dev";
        if (env.IsStaging())
            serviceName += "-stag";

        Id = $"{serviceName}-{DateTime.Now.GetTotalMilliseconds()}";
        ServiceName = serviceName;
        ShortName = fullName.Split(".")[^2];
        CorsPolicy = "default";
        StartAssembly = startAssembly;
        Description = description;
        Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision:00}";
    }
}