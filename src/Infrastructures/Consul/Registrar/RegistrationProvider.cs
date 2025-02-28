using Adnc.Infra.Consul.Configuration;

namespace Adnc.Infra.Consul.Registrar;

public sealed class RegistrationProvider
{
    private readonly IOptions<ConsulOptions> _consulConfig;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<RegistrationProvider> _logger;
    private readonly ConsulClient _consulClient;
    //private readonly IServerAddressesFeature _serverAddressesFeature;

    public RegistrationProvider(
        IOptions<ConsulOptions> consulOption
        , ConsulClient consulClient
        , IHostApplicationLifetime hostApplicationLifetime
        , ILogger<RegistrationProvider> logger)
    {
        _consulConfig = consulOption;
        _consulClient = consulClient;
        _hostApplicationLifetime = hostApplicationLifetime;
        //_serverAddressesFeature = serviceProvider.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
        _logger = logger;
    }

    public void Register(Uri serviceAddress, string? serviceId = null)
    {
        if (serviceAddress is null)
            throw new ArgumentNullException(nameof(serviceAddress));

        var instance = GetAgentServiceRegistration(serviceAddress, serviceId);
        Register(instance);
    }

    public void Register(AgentServiceRegistration instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        CheckConfig();
        var protocol = instance.Meta["Protocol"];
        _logger.LogInformation(@$"register to consul ({protocol}://{instance.Address}:{instance.Port})");
        _hostApplicationLifetime.ApplicationStarted.Register(async () => await _consulClient.Agent.ServiceRegister(instance));
        _hostApplicationLifetime.ApplicationStopping.Register(async () => await _consulClient.Agent.ServiceDeregister(instance.ID));
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
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);

        var IPList = new List<string>();
        if (netType == string.Empty)
        {
            for (int i = 0; i < addresses.Length; i++)
            {
                IPList.Add(addresses[i].ToString());
            }
        }
        else
        {
            //AddressFamily.InterNetwork = IPv4,
            //AddressFamily.InterNetworkV6= IPv6
            for (int i = 0; i < addresses.Length; i++)
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
        if (serviceAddress is null)
            throw new ArgumentNullException(nameof(serviceAddress));

        var protocol = serviceAddress.Scheme;
        var host = serviceAddress.Host;
        var port = serviceAddress.Port;
        var registrationInstance = new AgentServiceRegistration()
        {
            ID = serviceId ?? $"{_consulConfig.Value.ServiceName}-{DateTime.Now.GetTotalMilliseconds()}",
            Name = _consulConfig.Value.ServiceName,
            Address = host,
            Port = port,
            Meta = new Dictionary<string, string>() { ["Protocol"] = protocol },
            Tags = _consulConfig.Value.ServerTags,
            Check = new AgentServiceCheck
            {
                //服务停止多久后进行注销
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(_consulConfig.Value.DeregisterCriticalServiceAfter),
                //健康检查间隔,心跳间隔
                Interval = TimeSpan.FromSeconds(_consulConfig.Value.HealthCheckIntervalInSecond),
                //健康检查地址
                HTTP = $"{protocol}://{host}:{port}/{_consulConfig.Value.HealthCheckUrl}",
                //超时时间
                Timeout = TimeSpan.FromSeconds(_consulConfig.Value.Timeout),
            }
        };
        return registrationInstance;
    }

    private void CheckConfig()
    {
        if (_consulConfig == null)
            throw new ArgumentException(nameof(_consulConfig));
        if (string.IsNullOrEmpty(_consulConfig.Value.ConsulUrl))
            throw new ArgumentException(nameof(_consulConfig.Value.ConsulUrl));
        if (string.IsNullOrEmpty(_consulConfig.Value.ServiceName))
            throw new ArgumentException(nameof(_consulConfig.Value.ServiceName));
        if (string.IsNullOrEmpty(_consulConfig.Value.HealthCheckUrl))
            throw new ArgumentException(nameof(_consulConfig.Value.HealthCheckUrl));
        if (_consulConfig.Value.HealthCheckIntervalInSecond <= 0)
            throw new ArgumentException(nameof(_consulConfig.Value.HealthCheckIntervalInSecond));
        if (_consulConfig.Value.DeregisterCriticalServiceAfter <= 0)
            throw new ArgumentException(nameof(_consulConfig.Value.DeregisterCriticalServiceAfter));
        if (_consulConfig.Value.Timeout <= 0)
            throw new ArgumentException(nameof(_consulConfig.Value.Timeout));
    }
}