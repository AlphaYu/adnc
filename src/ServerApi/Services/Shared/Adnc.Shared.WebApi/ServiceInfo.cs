namespace Adnc.Shared.WebApi;

public sealed class ServiceInfo : IServiceInfo
{
    private static ServiceInfo _instance = null;
    private static readonly object _lockObj = new();

    public string Id { get; private set; }
    public string CorsPolicy { get; set; }
    public string ShortName { get; private set; }
    public string FullName { get; private set; }
    public string Version { get; private set; }
    public string Description { get; private set; }
    public string AssemblyName { get; private set; }
    public string AssemblyFullName { get; private set; }
    public string AssemblyLocation { get; private set; }

    private ServiceInfo()
    {
    }

    static ServiceInfo()
    {
    }

    public static ServiceInfo GetInstance(Assembly startAssembly)
    {
        if (_instance == null)
        {
            lock (_lockObj)
            {
                if (_instance == null)
                {
                    if(startAssembly is null)
                        startAssembly = Assembly.GetEntryAssembly();
                    var description = startAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                    var assemblyName = startAssembly.GetName();
                    var version = assemblyName.Version;
                    var fullName = assemblyName.Name.ToLower();
                    var splitFullName = fullName.Split(".");
                    var shortName = splitFullName[^2];

                    _instance = new ServiceInfo
                    {
                        Id = DateTime.Now.GetTotalMilliseconds().ToString(),
                        CorsPolicy = "default",
                        FullName = fullName,
                        ShortName = shortName,
                        AssemblyName = assemblyName.Name,
                        AssemblyFullName = startAssembly.FullName,
                        AssemblyLocation = startAssembly.Location,
                        Description = description,
                        Version = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision:00}"
                    };
                }
            }
        }

        return _instance;
    }
}