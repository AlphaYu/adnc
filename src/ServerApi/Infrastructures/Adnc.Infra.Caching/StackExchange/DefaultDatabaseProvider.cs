using Adnc.Infra.Caching.Configurations;
using Adnc.Infra.Caching.Core;
using StackExchange.Redis;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Redis database provider.
    /// </summary>
    public class DefaultDatabaseProvider
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly IOptions<CacheOptions> _options;

        /// <summary>
        /// The connection multiplexer.
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public DefaultDatabaseProvider(IOptions<CacheOptions> options)
        {
            _options = options;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        public string DBProviderName => CachingConstValue.Provider.StackExchange;

        /// <summary>
        /// Gets the database connection.
        /// </summary>
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.Value.GetDatabase();
        }

        /// <summary>
        /// Gets the server list.
        /// </summary>
        /// <returns>The server list.</returns>
        public IEnumerable<IServer> GetServerList()
        {
            var endpoints = GetMastersServersEndpoints();

            foreach (var endpoint in endpoints)
            {
                yield return _connectionMultiplexer.Value.GetServer(endpoint);
            }
        }

        /// <summary>
        /// Creates the connection multiplexer.
        /// </summary>
        /// <returns>The connection multiplexer.</returns>
        private ConnectionMultiplexer CreateConnectionMultiplexer()
        {
            var dbconfig = _options.Value.DBConfig;
            if (string.IsNullOrWhiteSpace(dbconfig.ConnectionString))
            {
                var configurationOptions = new ConfigurationOptions
                {
                    ConnectTimeout = dbconfig.ConnectionTimeout,
                    User = dbconfig.Username,
                    Password = dbconfig.Password,
                    Ssl = dbconfig.IsSsl,
                    SslHost = dbconfig.SslHost,
                    AllowAdmin = dbconfig.AllowAdmin,
                    DefaultDatabase = dbconfig.Database,
                    AbortOnConnectFail = dbconfig.AbortOnConnectFail,
                };

                foreach (var endpoint in dbconfig.Endpoints)
                {
                    configurationOptions.EndPoints.Add(endpoint.Host, endpoint.Port);
                }

                return ConnectionMultiplexer.Connect(configurationOptions.ToString());
            }
            else
            {
                _ = ConfigurationOptions.Parse(dbconfig.ConnectionString);
                return ConnectionMultiplexer.Connect(dbconfig.ConnectionString);
            }
        }

        /// <summary>
        /// Gets the masters servers endpoints.
        /// </summary>
        private List<EndPoint> GetMastersServersEndpoints()
        {
            var masters = new List<EndPoint>();
            foreach (var ep in _connectionMultiplexer.Value.GetEndPoints())
            {
                var server = _connectionMultiplexer.Value.GetServer(ep);
                if (server.IsConnected)
                {
                    //Cluster
                    if (server.ServerType == ServerType.Cluster)
                    {
                        masters.AddRange(server.ClusterConfiguration.Nodes.Where(n => !n.IsReplica).Select(n => n.EndPoint));
                        break;
                    }
                    // Single , Master-Slave
                    if (server.ServerType == ServerType.Standalone && !server.IsReplica)
                    {
                        masters.Add(ep);
                        break;
                    }
                }
            }
            return masters;
        }
    }
}