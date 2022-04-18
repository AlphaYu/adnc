using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adnc.Infra.Consul.Registration
{
    public static class ConsulRegistration
    {
        public static void Register(IApplicationBuilder app)
        {
            var consulOption = app.ApplicationServices.GetRequiredService<IOptions<ConsulConfig>>().Value;
            var consulClient = new ConsulClient(cfg => cfg.Address = new Uri(consulOption.ConsulUrl));

            var serviceAddress = GetServiceAddressInternal(app, consulOption);
            var serverId = $"{consulOption.ServiceName}.{DateTime.Now.Ticks}";

            var logger = app.ApplicationServices.GetRequiredService<ILogger<ConsulConfig>>();
            logger.LogInformation("serviceAddress:{0}:", serviceAddress);

            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() =>
            {
                var protocol = serviceAddress.Scheme;
                var host = serviceAddress.Host;
                var port = serviceAddress.Port;
                var check = new AgentServiceCheck
                {
                    //服务停止多久后进行注销
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60),
                    //健康检查间隔,心跳间隔
                    Interval = TimeSpan.FromSeconds(6),
                    //健康检查地址
                    HTTP = $"{protocol}://{host}:{port}/{consulOption.HealthCheckUrl}",
                    //超时时间
                    Timeout = TimeSpan.FromSeconds(6),
                };

                var registration = new AgentServiceRegistration()
                {
                    ID = serverId,
                    Name = consulOption.ServiceName,
                    Address = host,
                    Port = port,
                    Meta = new Dictionary<string, string>() { ["Protocol"] = protocol },
                    Tags = consulOption.ServerTags,
                    Check = check
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serverId).Wait();
            });
        }

        public static Uri GetServiceAddress(IApplicationBuilder app, ConsulConfig consulOption)
        {
            return GetServiceAddressInternal(app, consulOption);
        }

        private static Uri GetServiceAddressInternal(IApplicationBuilder app, ConsulConfig consulOption)
        {
            var errorMsg = string.Empty;
            Uri serviceAddress = default;

            if (consulOption == null)
                throw new Exception("请正确配置Consul");

            if (string.IsNullOrEmpty(consulOption.ConsulUrl))
                throw new Exception("请正确配置ConsulUrl");

            if (string.IsNullOrEmpty(consulOption.ServiceName))
                throw new Exception("请正确配置ServiceName");

            if (string.IsNullOrEmpty(consulOption.HealthCheckUrl))
                throw new Exception("请正确配置HealthCheckUrl");

            if (consulOption.HealthCheckIntervalInSecond <= 0)
                throw new Exception("请正确配置HealthCheckIntervalInSecond");

            //获取网卡所有Ip地址，排除回路地址
            var allIPAddress = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                               .Select(p => p.GetIPProperties())
                               .SelectMany(p => p.UnicastAddresses)
                               .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !System.Net.IPAddress.IsLoopback(p.Address))
                               .Select(p => p.Address.ToString()).ToArray();

            //获取web服务器监听地址，也就是提供访问的地址
            var listenAddresses = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.ToList();
            List<Uri> listenUrls = new List<Uri>();
            listenAddresses.ForEach(a =>
            {
                var address = a.Replace("[::]", "0.0.0.0")
                               .Replace("+", "0.0.0.0")
                               .Replace("*", "0.0.0.0");

                listenUrls.Add(new Uri(address));
            });

            var logger = app.ApplicationServices.GetRequiredService<ILogger<ConsulConfig>>();

            //第一种注册方式，在配置文件中指定服务地址
            //如果配置了服务地址, 只需要检测是否在listenUrls里面即可
            if (!string.IsNullOrEmpty(consulOption.ServiceUrl))
            {
                logger.LogInformation("consulOption.ServiceUrl:{0}", consulOption.ServiceUrl);

                serviceAddress = new Uri(consulOption.ServiceUrl);
                bool isExists = listenUrls.Where(p => p.Host == serviceAddress.Host || p.Host == "0.0.0.0").Any();
                if (isExists)
                    return serviceAddress;
                else
                    throw new Exception($"服务{consulOption.ServiceUrl}配置错误 listenUrls={string.Join(',', (IEnumerable<Uri>)listenUrls)}");
            }

            //第二种注册方式，服务地址通过docker环境变量(DOCKER_LISTEN_HOSTANDPORT)指定。
            //可以写在dockerfile文件中，也可以运行容器时指定。运行容器时指定才是最合理的,大家看各自的情况怎么处理吧。
            var dockerListenServiceUrl = Environment.GetEnvironmentVariable("DOCKER_LISTEN_HOSTANDPORT");
            if (!string.IsNullOrEmpty(dockerListenServiceUrl))
            {
                logger.LogInformation("dockerListenServiceUrl:{0}", dockerListenServiceUrl);
                serviceAddress = new Uri(dockerListenServiceUrl);
                return serviceAddress;
            }

            //第三种注册方式，注册程序自动获取服务地址
            //本机所有可用IP与listenUrls进行匹配, 如果listenUrl是"0.0.0.0"或"[::]", 则任意IP都符合匹配
            var matches = allIPAddress.SelectMany(ip =>
                                      listenUrls
                                      .Where(uri => ip == uri.Host || uri.Host == "0.0.0.0")
                                      .Select(uri => new { Protocol = uri.Scheme, ServiceIP = ip, Port = uri.Port })
                                      ).ToList();

            //过滤无效地址
            var filteredMatches = matches.Where(p => !p.ServiceIP.Contains("0.0.0.0")
                                                && !p.ServiceIP.Contains("localhost")
                                                && !p.ServiceIP.Contains("127.0.0.1")
                                                );

            var finalMatches = filteredMatches.ToList();

            //没有匹配的地址,抛出异常
            if (finalMatches.Count() == 0)
                throw new Exception($"没有匹配的Ip地址=[{string.Join(',', allIPAddress)}], urls={string.Join(',', (IEnumerable<Uri>)listenUrls)}");

            //只有一个匹配,直接返回
            if (finalMatches.Count() == 1)
            {
                serviceAddress = new Uri($"{finalMatches[0].Protocol}://{ finalMatches[0].ServiceIP}:{finalMatches[0].Port}");
                logger.LogInformation("serviceAddress:{0}", serviceAddress);
                return serviceAddress;
            }

            //匹配多个，直接返回第一个
            serviceAddress = new Uri($"{finalMatches[0].Protocol}://{ finalMatches[0].ServiceIP}:{finalMatches[0].Port}");
            logger.LogInformation("serviceAddress-first:{0}", serviceAddress);
            return serviceAddress;
        }
    }
}