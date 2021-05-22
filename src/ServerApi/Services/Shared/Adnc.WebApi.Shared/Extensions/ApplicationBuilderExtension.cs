﻿using Adnc.Infra.Consul;
using Adnc.WebApi.Shared;
using Adnc.WebApi.Shared.Middleware;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// 统一注册Adnc.WebApi通用中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <param name="serviceInfo"></param>
        /// <param name="completedExecute"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAdncMiddlewares(this IApplicationBuilder app, Action<IApplicationBuilder> completedExecute = null)
        {
            var configuration = app.ApplicationServices.GetService<IConfiguration>();
            var environment = app.ApplicationServices.GetService<IWebHostEnvironment>();
            var serviceInfo = app.ApplicationServices.GetService<ServiceInfo>();

            if (environment.IsDevelopment()) IdentityModelEventSource.ShowPII = true;

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);

            app.UseStaticFiles();

            app.UseCustomExceptionHandler();

            app.UseRealIp(x =>
            {
                x.HeaderKeys = new string[] { "X-Forwarded-For", "X-Real-IP" };
            });

            app.UseCors(serviceInfo.CorsPolicy);

            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"/{serviceInfo.ShortName}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description = serviceInfo.Description } };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{serviceInfo.ShortName}/swagger/{serviceInfo.Version}/swagger.json", $"{serviceInfo.FullName}-{serviceInfo.Version}");
                c.RoutePrefix = $"{serviceInfo.ShortName}";
            });

            var healthUrl = configuration.GetConsulConfig().HealthCheckUrl;
            app.UseHealthChecks($"/{healthUrl}", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseRouting();

            app.UseAuthentication();

            var iSSOAuthentication = configuration.IsSSOAuthentication();
            app.UseSSOAuthentication(iSSOAuthentication);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });

            completedExecute?.Invoke(app);

            return app;
        }

        /// <summary>
        /// 注册Cap节点到consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="consulConfig"></param>
        /// <param name="serviceInfo"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterCapToConsul(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<IConfiguration>();
            var consulConfig = configuration.GetConsulConfig();
            var serviceInfo = app.ApplicationServices.GetService<ServiceInfo>();

            var consulAdderss = new Uri(consulConfig.ConsulUrl);
            var discoverOptions = app.ApplicationServices.GetService<DiscoveryOptions>();
            var currenServerAddress = app.GetServiceAddress(consulConfig);
            var logger = app.ApplicationServices.GetRequiredService<ILogger<SharedServicesRegistration>>();
            logger.LogInformation("CapServiceAddress:{0}:", $"{ currenServerAddress.Host}:{ currenServerAddress.Port }");
            discoverOptions.DiscoveryServerHostName = consulAdderss.Host;
            discoverOptions.DiscoveryServerPort = consulAdderss.Port;
            discoverOptions.CurrentNodeHostName = currenServerAddress.Host;
            discoverOptions.CurrentNodePort = currenServerAddress.Port;
            discoverOptions.NodeId = currenServerAddress.Host.Replace(".", string.Empty) + currenServerAddress.Port;
            discoverOptions.NodeName = serviceInfo.FullName.Replace("webapi", "cap");
            discoverOptions.MatchPath = $"/{serviceInfo.ShortName}/cap";

            return app;
        }
    }
}