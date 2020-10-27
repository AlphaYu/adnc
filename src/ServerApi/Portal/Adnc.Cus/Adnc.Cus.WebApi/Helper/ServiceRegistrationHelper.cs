using System;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Polly;
using Adnc.Infr.EfCore.Interceptors;
using Adnc.Infr.Mongo;
using Adnc.Infr.Mongo.Extensions;
using Adnc.Infr.Mongo.Configuration;
using Adnc.Infr.EfCore;
using Adnc.WebApi.Shared;
using Adnc.Core.Shared.RpcServices;
using Adnc.Cus.Core.EventBus;
using Adnc.Cus.Core;

namespace Adnc.Cus.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper : SharedServicesRegistration
    {
        public ServiceRegistrationHelper(IConfiguration configuration
            , IServiceCollection services
            , IWebHostEnvironment env
            , ServiceInfo serviceInfo)
          : base(configuration, services, env, serviceInfo)
        {
        }

        public override void AddControllers()
        {
            _services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                        //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
        }

        public override void AddEfCoreContext()
        {
            _services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(_mysqlConfig.WriteDbConnectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
                    mySqlOptions.MinBatchSize(2);
                    mySqlOptions.MigrationsAssembly(_serviceInfo.AssemblyName.Replace("WebApi", "Migrations"));
                });
                options.AddInterceptors(new CustomCommandInterceptor());
            });

        }

        public override void AddMongoContext()
        {
            _services.AddMongo<MongoContext>(options =>
            {
                options.ConnectionString = _mongoConfig.ConnectionStrings;
                options.PluralizeCollectionNames = _mongoConfig.PluralizeCollectionNames;
                options.CollectionNamingConvention = (NamingConvention)_mongoConfig.CollectionNamingConvention;
            });
        }

        public override void AddMqHostedServices()
        {
        }

        public void AddAllRpcService()
        {
            //重试策略
            var retryPolicy = Policy.Handle<HttpRequestException>()
                                    .OrResult<HttpResponseMessage>(response => response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                                    .WaitAndRetryAsync(new[]
                                    {
                                        TimeSpan.FromSeconds(1),
                                        TimeSpan.FromSeconds(5),
                                        TimeSpan.FromSeconds(10)
                                    });
            //超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(30);
            //隔离策略
            var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);
            //回退策略
            //断路策略
            var circuitBreakerPolicy = Policy.Handle<Exception>()
                                     .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

            var policies = new List<IAsyncPolicy<HttpResponseMessage>>()
            {
                timeoutPolicy
            };

            //注册用户认证、鉴权服务Rpc服务到容器
            var authServerAddress = (_env.IsProduction() || _env.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            base.AddRpcService<IAuthRpcService>(authServerAddress, policies);
        }

        public override void AddEventBusSubscribers(string tableNamePrefix="Cap", string groupName= EbConsts.CapDefaultGroup)
        {
            base.AddEventBusSubscribers(tableNamePrefix, groupName);
            _services.AddScoped<IRechargeSubscriber, RechargeSubscriber>();
        }
    }
}
