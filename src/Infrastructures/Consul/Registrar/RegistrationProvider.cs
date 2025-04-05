using Adnc.Infra.Consul.Configuration;

namespace Adnc.Infra.Consul.Registrar;

public sealed class RegistrationProvider(IOptions<ConsulOptions> consulOption, ConsulClient consulClient, IHostApplicationLifetime hostApplicationLifetime, ILogger<RegistrationProvider> logger)
{
    public void Register(Uri serviceUri, string? serviceId = null)
    {
        ArgumentNullException.ThrowIfNull(serviceUri);

        var instance = GetAgentServiceRegistration(serviceUri, serviceId);
        Register(instance);
    }

    public void Register(AgentServiceRegistration instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        CheckConfig();
        var protocol = instance.Meta["Protocol"];
        var address = instance.Address;
        var port = instance.Port;
        logger.LogInformation("register to consul ({protocol}://{address}:{port})", protocol, address, port);
        hostApplicationLifetime.ApplicationStarted.Register(async () => await consulClient.Agent.ServiceRegister(instance));
        hostApplicationLifetime.ApplicationStopping.Register(async () => await consulClient.Agent.ServiceDeregister(instance.ID));
    }

    /// <summary>
    /// get all ip address
    /// </summary>
    public List<string> GetServerIpAddress()
    => System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                                       .Select(p => p.GetIPProperties())
                                       .SelectMany(p => p.UnicastAddresses)
                                       .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))
                                       .Select(p => p.Address.ToString()).ToList();

    /// <summary>
    /// get all ip address
    /// </summary>
    /// <param name="netType">"InterNetwork":ipv4，"InterNetworkV6":ipv6</param>
    public List<string> GetLocalIpAddress(string netType)
    {
        var hostName = Dns.GetHostName();
        var addresses = Dns.GetHostAddresses(hostName);

        var IPList = new List<string>();
        if (netType == string.Empty)
        {
            for (var i = 0; i < addresses.Length; i++)
            {
                IPList.Add(addresses[i].ToString());
            }
        }
        else
        {
            //AddressFamily.InterNetwork = IPv4,
            //AddressFamily.InterNetworkV6= IPv6
            for (var i = 0; i < addresses.Length; i++)
            {
                if (addresses[i].AddressFamily.ToString() == netType)
                {
                    IPList.Add(addresses[i].ToString());
                }
            }
        }
        return IPList;
    }

    private AgentServiceRegistration GetAgentServiceRegistration(Uri serviceAddress, string? serviceId = null)
    {
        ArgumentNullException.ThrowIfNull(serviceAddress);

        var protocol = serviceAddress.Scheme;
        var host = serviceAddress.Host;
        var port = serviceAddress.Port;
        var registrationInstance = new AgentServiceRegistration()
        {
            ID = serviceId ?? $"{consulOption.Value.ServiceName}-{DateTime.Now.GetTotalMilliseconds()}",
            Name = consulOption.Value.ServiceName,
            Address = host,
            Port = port,
            Meta = new Dictionary<string, string>() { ["Protocol"] = protocol },
            Tags = consulOption.Value.ServerTags,
            Check = new AgentServiceCheck
            {
                //服务停止多久后进行注销
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(consulOption.Value.DeregisterCriticalServiceAfter),
                //健康检查间隔,心跳间隔
                Interval = TimeSpan.FromSeconds(consulOption.Value.HealthCheckIntervalInSecond),
                //健康检查地址
                HTTP = $"{protocol}://{host}:{port}/{consulOption.Value.HealthCheckUrl}",
                //超时时间
                Timeout = TimeSpan.FromSeconds(consulOption.Value.Timeout),
            }
        };
        return registrationInstance;
    }

    private void CheckConfig()
    {
        if (consulOption == null)
        {
            throw new ArgumentException(nameof(consulOption));
        }

        if (string.IsNullOrEmpty(consulOption.Value.ConsulUrl))
        {
            throw new ArgumentException(nameof(consulOption.Value.ConsulUrl));
        }

        if (string.IsNullOrEmpty(consulOption.Value.ServiceName))
        {
            throw new ArgumentException(nameof(consulOption.Value.ServiceName));
        }

        if (string.IsNullOrEmpty(consulOption.Value.HealthCheckUrl))
        {
            throw new ArgumentException(nameof(consulOption.Value.HealthCheckUrl));
        }

        if (consulOption.Value.HealthCheckIntervalInSecond <= 0)
        {
            throw new ArgumentException(nameof(consulOption.Value.HealthCheckIntervalInSecond));
        }

        if (consulOption.Value.DeregisterCriticalServiceAfter <= 0)
        {
            throw new ArgumentException(nameof(consulOption.Value.DeregisterCriticalServiceAfter));
        }

        if (consulOption.Value.Timeout <= 0)
        {
            throw new ArgumentException(nameof(consulOption.Value.Timeout));
        }
    }
}
