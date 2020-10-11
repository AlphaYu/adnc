using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Consul;

namespace Adnc.Infr.Consul.Registration
{
    public static class RegistrationExtension
    {
        public static void RegisterToConsul(this IApplicationBuilder app, ConsulConfig consulOption)
        {

            CheckConfig(consulOption);

            var serverId = $"{consulOption.ServiceName}-{(DateTime.UtcNow.Ticks - 621355968000000000) / 10000000}";

            var consulClient = new ConsulClient(ConsulClientConfiguration => ConsulClientConfiguration.Address = new Uri(consulOption.ConsulUrl));

            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(() =>
            {
                if (!TryGetServiceUrl(app, consulOption, out var serverListenUrl, out var errorMsg))
                {
                    throw new Exception(errorMsg);
                }

                var protocol = serverListenUrl.Scheme;
                var host = serverListenUrl.Host;
                var port = serverListenUrl.Port;
                var check = new AgentServiceCheck
                {
                    ////服务启动多久后注册
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(3),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"{protocol}://{host}:{port}/{consulOption.HealthCheckUrl}",
                    //Timeout = TimeSpan.FromSeconds(5)
                };

                var registration = new AgentServiceRegistration()
                {
                    ID = serverId,
                    Name = consulOption.ServiceName,
                    Address = host,
                    Port = port,
                    Meta = new Dictionary<string, string>() { ["Protocol"] = protocol },
                    Tags = consulOption.ServerTags,
                    Checks = new AgentServiceCheck[] { check }
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serverId).Wait();
            });
        }

        private static void CheckConfig(ConsulConfig serviceOptions)
        {
            if (serviceOptions == null)
                throw new Exception("请正确配Consul");

            if (string.IsNullOrEmpty(serviceOptions.ConsulUrl))
                throw new Exception("请正确配置ConsulUrl");

            if (string.IsNullOrEmpty(serviceOptions.ServiceName))
                throw new Exception("请正确配置ServiceName");

            if (string.IsNullOrEmpty(serviceOptions.HealthCheckUrl))
                throw new Exception("请正确配置HealthCheckUrl");

            if (serviceOptions.HealthCheckIntervalInSecond <= 0)
                throw new Exception("请正确配置HealthCheckIntervalInSecond");

        }

        private static bool TryGetServiceUrl(IApplicationBuilder app, ConsulConfig consulOption, out Uri serverListenUrl, out string errorMsg)
        {
            errorMsg = string.Empty;
            serverListenUrl = default;

            var allIPAddress = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                               .Select(p => p.GetIPProperties())
                               .SelectMany(p => p.UnicastAddresses)
                               .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                               .Select(p => p.Address.ToString()).ToArray();

            //docker容器部署服务,从环境变量获取ServiceUriHost与Port
            if (consulOption.IsDocker)
            {
                var listenHostAndPort = Environment.GetEnvironmentVariable("DOCKER_LISTEN_HOSTANDPORT");
                if (string.IsNullOrEmpty(listenHostAndPort))
                {
                    errorMsg = "docker部署，必须配置DOCKER_LISTEN_HOSTANDPORT环境变量";
                    return false;
                }
                serverListenUrl = new Uri(listenHostAndPort);
                return true;
            }

            var listenUrls = app.ServerFeatures.Get<IServerAddressesFeature>()
                            .Addresses
                            .Select(url => new Uri(url)).ToArray();

            //不是docker容器部署，并且没有配置ServiceUriHost
            if (string.IsNullOrWhiteSpace(consulOption.ServiceUrl))
            {
                // 本机所有可用IP与listenUrls进行匹配, 如果listenUrl是"0.0.0.0"或"[::]", 则任意IP都符合匹配
                var matches = allIPAddress.SelectMany(ip =>
                                          listenUrls
                                          .Where(uri => ip == uri.Host || uri.Host == "0.0.0.0" || uri.Host == "[::]")
                                          .Select(uri => new { Protocol = uri.Scheme, ServiceIP = ip, Port = uri.Port })
                                          ).ToList();

                if (matches.Count == 0)
                {
                    errorMsg = $"没有匹配的Ip地址=[{string.Join(',', allIPAddress)}], urls={string.Join(',', (IEnumerable<Uri>)listenUrls)}.";
                    return false;
                }
                else if (matches.Count == 1)
                {
                    serverListenUrl = new Uri($"{matches[0].Protocol}://{ matches[0].ServiceIP}:{matches[0].Port}");
                    return true;
                }
                else
                {
                    errorMsg = $"请指定ServiceUrl: {string.Join(",", matches)}.";
                    return false;
                }
            }

            // 如果提供了对外服务的IP, 只需要检测是否在listenUrls里面即可
            var serviceUri = new Uri(consulOption.ServiceUrl);
            bool isExists = listenUrls.Where(p => p.Host == serviceUri.Host || p.Host == "0.0.0.0" || p.Host == "[::]").Any();
            if (isExists)
            {
                serverListenUrl = serviceUri;
                return true;
            }
            errorMsg = $"服务Ip配置错误 urls={string.Join(',', (IEnumerable<Uri>)listenUrls)}";
            return false;
        }
    }
}
