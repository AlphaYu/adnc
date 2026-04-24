using Consul;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace youapi;

internal static class Registers
{
    public static string? Address { get => GetContainerIPv4(); }

    public static async Task Register(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var port = GetHttpPortFromApplicationUrls(configuration) ?? 8080;
        var address = GetContainerIPv4() ?? "localhost";
        var consul = configuration["Consul"] ?? "http://localhost:8500";

        var consulClient = new ConsulClient(config =>
        {
            config.Address = new Uri(consul); // Consul 地址
        });

        var registration = new AgentServiceRegistration()
        {
            ID = "my-api-1", // 唯一ID
            Name = "my-api", // 服务名
            Address = address, // 主机地址
            Port = port, // 服务端口
            Check = new AgentServiceCheck()
            {
                HTTP = $"http://{address}:{port}/health", // 健康检查地址
                Interval = TimeSpan.FromSeconds(10),
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30) // 连续失败后注销
            }
        };
        try
        {
            await consulClient.Agent.ServiceRegister(registration).WaitAsync(TimeSpan.FromSeconds(10));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"注册到 Consul 失败！错误信息: {ex.Message}，Url：{consul}");
            Console.WriteLine($"堆栈信息: {ex.StackTrace}");
        }
        // 应用关闭时注销
        lifetime.ApplicationStopping.Register(() =>
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
    }

    private static string? GetContainerIPv4()
    {
        string? ipv4 = null;

        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
            {
                ipv4 = ip.ToString();
                break;
            }
        }

        return ipv4;
    }

    public static int? GetHttpPortFromApplicationUrls(this IConfiguration configuration)
    {
        string profile = Environment.GetEnvironmentVariable("DOTNET_LAUNCH_PROFILE")
                 ?? configuration["DOTNET_LAUNCH_PROFILE"] ?? "default";

        string? httpUrl = configuration["profiles:http:applicationUrl"];
        string? httpsUrl = configuration["profiles:https:applicationUrl"];
        string? profileUrl = configuration[$"profiles:{profile}:applicationUrl"];

        string applicationUrls = profileUrl ?? ((httpsUrl ?? httpUrl) ?? configuration["urls"])
                   ?? "http://localhost:8080"; // 默认地址

        var urls = applicationUrls.Split(';', StringSplitOptions.RemoveEmptyEntries);
        var http = urls.FirstOrDefault(l => l.StartsWith("http://", StringComparison.OrdinalIgnoreCase));

        if (http != null && Uri.TryCreate(http, UriKind.Absolute, out var uri))
        {
            return uri.Port;
        }

        return null;
    }

    internal static TBuilder UseLaunchSettings<TBuilder>(this TBuilder builder, string? resourceName = null)
       where TBuilder : IHostApplicationBuilder
    {
        string? launchProfilePath = ProjectSourcePath.Value;
        if (!string.IsNullOrEmpty(builder.Configuration["DOTNET_RUNNING_IN_CONTAINER"]))
        {
            return builder;
        }

        if (string.IsNullOrEmpty(launchProfilePath))
        {
            throw new InvalidOperationException("Content root path is not set.");
        }

        string basePath =
           resourceName is null ? launchProfilePath : (Directory.GetParent(launchProfilePath)?.FullName ?? launchProfilePath) + resourceName;
        string launchSettingsFilePath = Path.Combine(basePath, "Properties", "launchSettings.json");

        try
        {
            // It isn't mandatory that the launchSettings.json file exists!
            if (!File.Exists(launchSettingsFilePath))
            {
                throw new FileNotFoundException($"The launch settings file '{launchSettingsFilePath}' does not exist.");
            }

            // 1. 读取 launchSettings.json
            IConfigurationRoot launchSettings = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Properties/launchSettings.json", optional: false)
                .Build();
            _ = builder.Configuration.AddConfiguration(launchSettings);
            return builder;
        }
        catch (FileNotFoundException ex)
        {
            string message = $"Failed to get effective launch profile for project resource '{resourceName}'. An unexpected error occurred while loading the launch settings file '{launchSettingsFilePath}': {ex.Message}";
            throw new FileNotFoundException(message, ex);
        }
    }

    ///<summary>
    ///Provides the full path to the source directory of the current project.<br/>
    ///(Only meaningful on the machine where this code was compiled.)<br/>
    ///From <a href="https://stackoverflow.com/a/66285728/773113"/>
    ///</summary>
    internal static class ProjectSourcePath
    {
        ///<summary>
        ///The full path to the source directory of the current project.
        ///</summary>
        public static string? Value => Calculate();

        private static string? Calculate([System.Runtime.CompilerServices.CallerFilePath] string? path = null)
        {
            string? appName = Assembly.GetExecutingAssembly().GetName().Name;

            if (appName is null || path is null)
            {
                return null;
            }

            DirectoryInfo? dir = new(path);
            while (dir != null && dir.Name != appName)
            {
                dir = dir.Parent;
            }
            return dir?.FullName;
        }
    }
}