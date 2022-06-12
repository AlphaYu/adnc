namespace Adnc.Shared.WebApi;

public class ServiceInfo : IServiceInfo
{
    public string Id { get; private set; }
    public string CorsPolicy { get; set; }
    public string ShortName { get; private set; }
    public string FullName { get; private set; }
    public string Version { get; private set; }
    public string Description { get; private set; }
    public Assembly StartAssembly { get; private set; }

    public ServiceInfo(Assembly startAssembly)
    {
        if (startAssembly is null)
            startAssembly = Assembly.GetEntryAssembly();
        var description = startAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
        var assemblyName = startAssembly.GetName();
        var version = assemblyName.Version;

        CorsPolicy = "default";
        FullName = assemblyName.Name.ToLower(); ;
        ShortName = this.FullName.Split(".")[^2];
        Id = $"{this.FullName.Replace(".", "-")}-{DateTime.Now.GetTotalMilliseconds()}";
        StartAssembly = startAssembly;
        Description = description;
        Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision:00}";
    }
}