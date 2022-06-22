namespace Adnc.Infra.Consul.Registrar;

public sealed class ConsulRegistration
{
    private readonly ConsulConfig _consulConfig;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<ConsulRegistration> _logger;
    private readonly ConsulClient _consulClient;
    //private readonly IServerAddressesFeature _serverAddressesFeature;

    public ConsulRegistration(
        IOptions<ConsulConfig> consulOption
        , ConsulClient consulClient
        , IHostApplicationLifetime hostApplicationLifetime
        , ILogger<ConsulRegistration> logger)
    {
        _consulConfig = consulOption.Value;
        _consulClient = consulClient;
        _hostApplicationLifetime = hostApplicationLifetime;
        //_serverAddressesFeature = serviceProvider.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>();
        _logger = logger;
    }

    public void Register(Uri serviceAddress,string? serviceId=null)
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

    private AgentServiceRegistration GetAgentServiceRegistration(Uri serviceAddress,string? serviceId = null)
    {
        if (serviceAddress is null)
            throw new ArgumentNullException(nameof(serviceAddress));

        var protocol = serviceAddress.Scheme;
        var host = serviceAddress.Host;
        var port = serviceAddress.Port;
        var registrationInstance = new AgentServiceRegistration()
        {
            ID = serviceId ?? $"{_consulConfig.ServiceName}-{DateTime.Now.GetTotalMilliseconds()}",
            Name = _consulConfig.ServiceName,
            Address = host,
            Port = port,
            Meta = new Dictionary<string, string>() { ["Protocol"] = protocol },
            Tags = _consulConfig.ServerTags,
            Check = new AgentServiceCheck
            {
                //服务停止多久后进行注销
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(_consulConfig.DeregisterCriticalServiceAfter),
                //健康检查间隔,心跳间隔
                Interval = TimeSpan.FromSeconds(_consulConfig.HealthCheckIntervalInSecond),
                //健康检查地址
                HTTP = $"{protocol}://{host}:{port}/{_consulConfig.HealthCheckUrl}",
                //超时时间
                Timeout = TimeSpan.FromSeconds(_consulConfig.Timeout),
            }
        };
        return registrationInstance;
    }

    private void CheckConfig()
    {
        if (_consulConfig == null)
            throw new ArgumentException(nameof(_consulConfig));
        if (string.IsNullOrEmpty(_consulConfig.ConsulUrl))
            throw new ArgumentException(nameof(_consulConfig.ConsulUrl));
        if (string.IsNullOrEmpty(_consulConfig.ServiceName))
            throw new ArgumentException(nameof(_consulConfig.ServiceName));
        if (string.IsNullOrEmpty(_consulConfig.HealthCheckUrl))
            throw new ArgumentException(nameof(_consulConfig.HealthCheckUrl));
        if (_consulConfig.HealthCheckIntervalInSecond <= 0)
            throw new ArgumentException(nameof(_consulConfig.HealthCheckIntervalInSecond));
        if (_consulConfig.DeregisterCriticalServiceAfter <= 0)
            throw new ArgumentException(nameof(_consulConfig.DeregisterCriticalServiceAfter));
        if (_consulConfig.Timeout <= 0)
            throw new ArgumentException(nameof(_consulConfig.Timeout));
    }
}