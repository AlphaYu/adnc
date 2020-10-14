using System.IO;
using System;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Autofac;
using AutoMapper;
using Adnc.Common;
using Adnc.Common.Helper;
using Adnc.Common.Models;
using Adnc.Infr.Consul.Registration;
using Adnc.Cus.WebApi.Helper;
using Adnc.Cus.Application;
using Adnc.Common.Consts;

namespace Adnc.Cus.WebApi
{
    public class Startup
    {
        private static readonly string _corsPolicy = "default";
        private static readonly string _serviceName = "cus";
        private static readonly string _serviceFullName = "adnc-cus";
        private static readonly string _description = "客户管理关Api";
        private static readonly string _version = "v0.5.0";
        private static readonly OpenApiInfo _openApiInfo = new OpenApiInfo { Title = _serviceName, Version = _version };
        private ServiceRegistrationHelper _srvRegistration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationHelper.Initialize(configuration);
        }

        public IConfiguration Configuration { get; }
        public IServiceCollection ServiceCollection { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollection = services;
            services.AddScoped<UserContext>();
            services.AddAutoMapper(typeof(AdncCusProfile));
            services.AddHttpContextAccessor();

            _srvRegistration = new ServiceRegistrationHelper(Configuration, services);
            _srvRegistration.Configure();
            _srvRegistration.AddControllers();
            _srvRegistration.AddJWTAuthentication();
            _srvRegistration.AddAuthorization();
            _srvRegistration.AddCors(_corsPolicy);
            _srvRegistration.AddHealthChecks();
            _srvRegistration.AddMqHostedServices();
            _srvRegistration.AddEfCoreContext();
            _srvRegistration.AddMongoContext();
            _srvRegistration.AddCaching(EasyCachingConsts.LocalCaching, EasyCachingConsts.RemoteCaching, EasyCachingConsts.HybridCaching, EasyCachingConsts.TopicName);
            _srvRegistration.AddSwaggerGen(_openApiInfo, new List<string>()
            {
                Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")
                ,Path.Combine(AppContext.BaseDirectory, "Adnc.Cus.Application.xml")
            });
            _srvRegistration.AddAllRpcService();
            _srvRegistration.AddEventBusSubscribers("Cap", EbConsts.CapDefaultGroup, _version);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册依赖模块
            builder.RegisterModule<Adnc.Infr.Mongo.AdncInfrMongoModule>();
            builder.RegisterModule<Adnc.Infr.EfCore.AdncInfrEfCoreModule>();
            builder.RegisterModule(new Adnc.Cus.Application.AdncCusApplicationModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ContainerContext.Initialize(new IocContainer(app.ApplicationServices));

            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                //开启验证异常显示
                //PII is hidden 异常处理
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders();
            app.UseCors(_corsPolicy);
            app.UseSwagger(c =>
            {
                c.RouteTemplate = $"/{_serviceName}/swagger/{{documentName}}/swagger.json";
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"/", Description = _description } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{_serviceName}/swagger/{_version}/swagger.json", $"{_serviceFullName}-{_version}");
                c.RoutePrefix = $"{_serviceName}";
            });
            //app.UseErrorHandling();
            app.UseHealthChecks($"/{_srvRegistration.GetConsulConfig().HealthCheckUrl}", new HealthCheckOptions()
            {
                Predicate = _ => true,
                // 该响应输出是一个json，包含所有检查项的详细检查结果
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCustomAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
            if (env.IsProduction() || env.IsStaging())
            {
                app.RegisterToConsul(_srvRegistration.GetConsulConfig());
            }
        }
    }
}