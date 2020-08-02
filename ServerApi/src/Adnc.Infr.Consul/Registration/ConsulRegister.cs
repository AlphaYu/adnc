using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adnc.Infr.Consul.Registration
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public static class Registration
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            //获取服务配置项
            var serviceOptions = app.ApplicationServices.GetRequiredService<IOptionsSnapshot<ConsulOption>>().Value;

            //检测配置
            CheckConfig(serviceOptions);

            var hostinfo = GetHostInfo(serviceOptions, app);
            // 服务ID，唯一的
            string serviceId = serviceOptions.ServiceName + Guid.NewGuid().ToString();
            //节点服务注册对象
            var registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = serviceOptions.ServiceName,  //对服务分组
                Address = hostinfo.Host, //服务地址
                Port = hostinfo.Port,
                Tags = new string[] { }, //标签信息，服务发现的时候可以获取到的，负载均衡策略扩展的
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(7),  //在7秒未连接上服务之后注销关键服务
                    Interval = TimeSpan.FromSeconds(2), //每个2秒发送一次心跳检测
                    Timeout = TimeSpan.FromSeconds(3),  //连接超时时间
                    HTTP = hostinfo.HealthUrl //   //心跳检测访问的接口地址，需要自己在项目中写好这个接口
                }
            };
            var consulClient = new ConsulClient(configuration =>
            {
                //服务注册地址：集群中任意一个地址
                configuration.Address = new Uri(serviceOptions.ConsulUrl);
            });
  

            //获取主机生命周期管理接口
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(() =>
            {
                //注册到consul
                consulClient.Agent.ServiceRegister(registration).Wait();
            });

            //程序停止的时候取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId).Wait();
            });

            return app;
        }


        /// <summary>
        /// 获取当前主机的域名和端口
        /// 用IIS启动获取不到。如果获取不到必须保证配置文件配了
        /// 启动的时候指定--Urls才可以获取到；例如 ：dotnet Service_One.dll --Urls "https://localhost:5002"
        /// </summary>
        /// <param name="serviceOptions"></param>
        /// <param name="app"></param>
        /// <returns>
        /// 第一个返回值：健康检查地址（例如：http://127.0.0.1:5001/healthcheck）
        /// 第二个返回值：主机地址(例如：127.0.0.1)
        /// 第三个返回值：端口号(例如：80)
        /// </returns>
        private static (string HealthUrl, string Host, int Port) GetHostInfo(ConsulOption serviceOptions, IApplicationBuilder app)
        {
            var features = app.Properties["server.Features"] as FeatureCollection;
            var address = features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            string http = null, host = null;
            int port = 0;
            if (!string.IsNullOrEmpty(address))
            {
                var uri = new Uri(address);// 协议头：uri.Sechema  主机：uri.Host  端口：uri.Port 
                http = address + serviceOptions.HealthCheck;
                host = uri.Scheme + "://" + uri.Host;
                port = uri.Port;
            }
            else
            {
                http = $"{serviceOptions.ServiceUriHost}:{serviceOptions.ServiceUriPort}{serviceOptions.HealthCheck}";
                host = serviceOptions.ServiceUriHost;
                port = serviceOptions.ServiceUriPort;
            }
            if (string.IsNullOrEmpty(host) || port < 1)
                throw new Exception("Consul配置未能获取到主机信息，请在consulconfig.json文件中配置");
            Console.WriteLine("健康检查地址：" + http);
            return (http, host, port);
        }

        /// <summary>
        /// 检查配置文件
        /// </summary>
        /// <param name="serviceOptions"></param>
        private static void CheckConfig(ConsulOption serviceOptions)
        {
            if (serviceOptions == null)
                throw new Exception("请正确配置consulconfig.json,其中包含ConsulAddress、ServiceName、HealthCheck");

            if (string.IsNullOrEmpty(serviceOptions.ConsulUrl))
                throw new Exception("请正确配置ConsulAddress");

            if (string.IsNullOrEmpty(serviceOptions.ServiceName))
                throw new Exception("请正确配置ServiceName");

            if (string.IsNullOrEmpty(serviceOptions.HealthCheck))
                throw new Exception("请正确配置HealthCheck");

        }
    }
}
