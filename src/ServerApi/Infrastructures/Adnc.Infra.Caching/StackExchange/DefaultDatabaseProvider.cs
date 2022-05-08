using Adnc.Infra.Caching.Configurations;
using StackExchange.Redis;
using System.Net;

namespace Adnc.Infra.Caching.StackExchange
{
    /// <summary>
    /// Redis database provider.
    /// </summary>
    public class DefaultDatabaseProvider : IRedisDatabaseProvider
    {
        /// <summary>
        /// The options.
        /// </summary>
        private readonly RedisDBOptions _options;

        /// <summary>
        /// The connection multiplexer.
        /// </summary>
        private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;

        public DefaultDatabaseProvider(CacheOptions options)
        {
            _options = options.DBConfig;
            _connectionMultiplexer = new Lazy<ConnectionMultiplexer>(CreateConnectionMultiplexer);
        }

        public string DBProviderName => "StackExchange";

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
            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
            {
                var configurationOptions = new ConfigurationOptions
                {
                    ConnectTimeout = _options.ConnectionTimeout,
                    User = _options.Username,
                    Password = _options.Password,
                    Ssl = _options.IsSsl,
                    SslHost = _options.SslHost,
                    AllowAdmin = _options.AllowAdmin,
                    DefaultDatabase = _options.Database,
                    AbortOnConnectFail = _options.AbortOnConnectFail,
                };

                foreach (var endpoint in _options.Endpoints)
                {
                    configurationOptions.EndPoints.Add(endpoint.Host, endpoint.Port);
                }

                return ConnectionMultiplexer.Connect(configurationOptions.ToString());
            }
            else
            {
                var options = ConfigurationOptions.Parse(_options.ConnectionString);
                return ConnectionMultiplexer.Connect(_options.ConnectionString);
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