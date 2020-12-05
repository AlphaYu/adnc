using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Net;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Polly;
using Refit;
using FluentValidation.AspNetCore;
using EasyCaching.InMemory;
using DotNetCore.CAP.Dashboard;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Swashbuckle.AspNetCore.Swagger;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Infr.Common;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Infr.Consul;
using Adnc.Infr.Consul.Consumer;
using Adnc.Infr.EfCore;
using Adnc.Infr.EfCore.Interceptors;
using Adnc.Infr.Mongo;
using Adnc.Infr.Mongo.Extensions;
using Adnc.Infr.Mongo.Configuration;
using Adnc.Application.Shared;
using Adnc.Application.Shared.RpcServices;
using Adnc.Infr.Common.Helper;
using Adnc.WebApi.Shared.Extensions;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using Microsoft.EntityFrameworkCore.Query;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;

namespace Adnc.WebApi.Shared
{
    public abstract class SharedServicesRegistration
    {
        protected readonly IConfiguration _cfg;
        protected readonly IServiceCollection _services;
        protected readonly IWebHostEnvironment _env;
        protected readonly ServiceInfo _serviceInfo;
        protected readonly JWTConfig _jwtConfig;
        protected readonly MongoConfig _mongoConfig;
        protected readonly MysqlConfig _mysqlConfig;
        protected readonly RedisConfig _redisConfig;
        protected readonly RabbitMqConfig _rabbitMqConfig;
        protected readonly ConsulConfig _consulConfig;
        protected readonly bool _isSSOAuthentication;

        /// <summary>
        /// 服务注册与系统配置
        /// </summary>
        /// <param name="cfg"><see cref="IConfiguration"/></param>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="env"><see cref="IWebHostEnvironment"/></param>
        /// <param name="serviceInfo"><see cref="ServiceInfo"/></param>
        public SharedServicesRegistration(IConfiguration cfg
            , IServiceCollection services
            , IWebHostEnvironment env
            , ServiceInfo serviceInfo)
        {
            _cfg = cfg;
            _env = env;
            _services = services;
            _serviceInfo = serviceInfo;

            //读取Jwt配置
            _jwtConfig = _cfg.GetSection("JWT").Get<JWTConfig>();
            //读取mongodb配置
            _mongoConfig = _cfg.GetSection("MongoDb").Get<MongoConfig>();
            //读取mysql配置
            _mysqlConfig = _cfg.GetSection("Mysql").Get<MysqlConfig>();
            //读取redis配置
            _redisConfig = _cfg.GetSection("Redis").Get<RedisConfig>();
            //读取rabbitmq配置
            _rabbitMqConfig = _cfg.GetSection("RabbitMq").Get<RabbitMqConfig>();
            //读取consul配置
            _consulConfig = _cfg.GetSection("Consul").Get<ConsulConfig>();
            //读取是否开启SSOAuthentication(单点登录验证)
            _isSSOAuthentication = _cfg.GetValue("SSOAuthentication", false);
        }

        /// <summary>
        /// 获取Jwt配置
        /// </summary>
        /// <returns><see cref="JWTConfig"/></returns>
        public virtual JWTConfig GetJWTConfig()
        {
            return _jwtConfig;
        }

        /// <summary>
        /// 获取mongdb配置
        /// </summary>
        /// <returns><see cref="MongoConfig"/></returns>
        public virtual MongoConfig GetMongoConfig()
        {
            return _mongoConfig;
        }

        /// <summary>
        /// 获取mysql配置
        /// </summary>
        /// <returns><see cref="MysqlConfig"/></returns>
        public virtual MysqlConfig GetMysqlConfig()
        {
            return _mysqlConfig;
        }

        /// <summary>
        /// 获取reids配置
        /// </summary>
        /// <returns><see cref="RedisConfig"/></returns>
        public virtual RedisConfig GetRedisConfig()
        {
            return _redisConfig;
        }

        /// <summary>
        /// 获取rabbitmq配置
        /// </summary>
        /// <returns><see cref="RabbitMqConfig"/></returns>
        public virtual RabbitMqConfig GetRabbitMqConfig()
        {
            return _rabbitMqConfig;
        }

        /// <summary>
        /// 获取consul配置
        /// </summary>
        /// <returns><see cref="ConsulConfig"/></returns>
        public virtual ConsulConfig GetConsulConfig()
        {
            return _consulConfig;
        }

        /// <summary>
        /// 获取SSOAuthentication是否开启
        /// </summary>
        public virtual bool IsSSOAuthentication { get { return _isSSOAuthentication; } }

        /// <summary>
        /// 注册配置类到IOC容器
        /// </summary>
        public virtual void Configure()
        {
            // 获取客户端真实Ip
            //https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.0#configuration-for-an-ipv4-address-represented-as-an-ipv6-address
            //_services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders =
            //        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            //});
            _services.Configure<JWTConfig>(_cfg.GetSection("JWT"));
            _services.Configure<MongoConfig>(_cfg.GetSection("MongoDb"));
            _services.Configure<MysqlConfig>(_cfg.GetSection("Mysql"));
            _services.Configure<RabbitMqConfig>(_cfg.GetSection("RabbitMq"));
        }

        /// <summary>
        /// Controllers 注册
        /// Sytem.Text.Json 配置
        /// FluentValidation 注册
        /// ApiBehaviorOptions 配置
        /// </summary>
        public virtual void AddControllers()
        {
            _services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                     .AddJsonOptions(options =>
                     {
                         options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                         options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                         options.JsonSerializerOptions.Encoder = SystemTextJsonHelper.GetAdncDefaultEncoder();
                         //该值指示是否允许、不允许或跳过注释。
                         options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                         //dynamic与匿名类型序列化设置
                         options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                         //dynamic
                         options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                         //匿名类型
                         options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                     })
                     .AddFluentValidation(cfg =>
                     {
                         //Continue 验证失败，继续验证其他项
                         cfg.ValidatorOptions.CascadeMode = FluentValidation.CascadeMode.Continue;
                         // Optionally set validator factory if you have problems with scope resolve inside validators.
                         // cfg.ValidatorFactoryType = typeof(HttpContextServiceProviderValidatorFactory);
                     });

            //参数验证返回信息格式调整
            _services.Configure<ApiBehaviorOptions>(options =>
            {
                //关闭自动验证
                //options.SuppressModelStateInvalidFilter = true;
                //格式化验证信息
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var problemDetails = new ProblemDetails
                    {
                        Detail = context.ModelState.GetValidationSummary("<br>")
                        ,
                        Title = "参数错误"
                        ,
                        Status = (int)HttpStatusCode.BadRequest
                        ,
                        Type = "https://httpstatuses.com/400"
                        ,
                        Instance = context.HttpContext.Request.Path
                    };

                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status
                    };
                };
            });
        }

        /// <summary>
        /// 注册EfcoreContext
        /// </summary>
        public virtual void AddEfCoreContext()
        {
            _services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(_mysqlConfig.WriteDbConnectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
                    mySqlOptions.MinBatchSize(2);
                    mySqlOptions.MigrationsAssembly(_serviceInfo.AssemblyName.Replace("WebApi", "Migrations"));
                });
                //替换默认查询sql生成器,为后面集成mycat实现读写分离做准备,
                //options.ReplaceService<IQuerySqlGeneratorFactory, AdncMySqlQuerySqlGeneratorFactory>();
            });
        }

        /// <summary>
        /// 注册MongdbContext
        /// </summary>
        public virtual void AddMongoContext()
        {
            _services.AddMongo<MongoContext>(options =>
            {
                options.ConnectionString = _mongoConfig.ConnectionStrings;
                options.PluralizeCollectionNames = _mongoConfig.PluralizeCollectionNames;
                options.CollectionNamingConvention = (NamingConvention)_mongoConfig.CollectionNamingConvention;
            });
        }

        /// <summary>
        /// 注册Jwt认证组件
        /// </summary>
        public virtual void AddJWTAuthentication()
        {
            //认证配置
            _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {

                //验证的一些设置，比如是否验证发布者，订阅者，密钥，以及生命时间等等
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SymmetricSecurityKey)),
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(_jwtConfig.ClockSkew)
                };
                options.Events = new JwtBearerEvents
                {
                    //接受到消息时调用
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    }
                    //在Token验证通过后调用
                    ,
                    OnTokenValidated = context =>
                    {
                        var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
                        var claims = context.Principal.Claims;
                        userContext.ID = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
                        userContext.Account = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                        userContext.Name = claims.First(x => x.Type == ClaimTypes.Name).Value;
                        userContext.Email = claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                        string[] roleIds = claims.First(x => x.Type == ClaimTypes.Role).Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        userContext.RoleIds = roleIds.Select(x => long.Parse(x)).ToArray();
                        userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                        return Task.CompletedTask;
                    }
                    //认证失败时调用
                    ,
                    OnAuthenticationFailed = context =>
                    {
                        //如果是过期，在http heard中加入act参数
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("act", "expired");
                        }
                        return Task.CompletedTask;
                    }
                    //未授权时调用
                    ,
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;

                        // Skip the default logic.
                        //context.HandleResponse();

                        //var payload = new JObject
                        //{
                        //    ["error"] = context.Error,
                        //    ["error_description"] = context.ErrorDescription,
                        //    ["error_uri"] = context.ErrorUri
                        //};

                        //return context.Response.WriteAsync(payload.ToString());
                    }
                };
            });

            //因为获取声明的方式默认是走微软定义的一套映射方式，如果我们想要走JWT映射声明，那么我们需要将默认映射方式给移除掉
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        /// <summary>
        /// 注册授权组件
        /// PermissionHandlerRemote 跨服务授权
        /// PermissionHandlerLocal  本地授权,adnc.usr走本地授权，其他服务走Rpc授权
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        public virtual void AddAuthorization<THandler>()
            where THandler: PermissionHandler
        {
            //自定义授权配置
            //services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            _services.AddAuthorization(options =>
            {
                options.AddPolicy(Permission.Policy, policy =>
                    policy.Requirements.Add(new PermissionRequirement()));
            });
            // 注册成全局 dbcontext 会报如下错误
            // A second operation started on this context before a previous operation completed.
            // This is usually caused by different threads using the same instance of DbContext. 
            // For more information on how to avoid threading issues with DbContext
            // see https://go.microsoft.com/fwlink/?linkid=2097913.
            //services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            _services.AddScoped<IAuthorizationHandler, THandler>();
        }

        /// <summary>
        /// 注册easycaching缓存组件
        /// </summary>
        /// <param name="localCacheName"></param>
        /// <param name="remoteCacheName"></param>
        /// <param name="hyBridCacheName"></param>
        /// <param name="topicName"></param>
        public virtual void AddCaching(string localCacheName = BaseEasyCachingConsts.LocalCaching
            , string remoteCacheName = BaseEasyCachingConsts.RemoteCaching
            , string hyBridCacheName = BaseEasyCachingConsts.HybridCaching
            , string topicName = BaseEasyCachingConsts.TopicName)
        {
            //初始化CSRedis，在系统中直接使用RedisHelper操作Redis
            //RedisHelper.Initialization(new CSRedis.CSRedisClient(Configuration.GetSection("Redis").Get<RedisConfig>().ConnectionString));
            //注册Redis用于系统Cache,但IDistributedCache接口提供的方法有限，只能存储Hash,如果需要其他操作直接使用RedisHelper
            //services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));

            //配置EasyCaching
            _services.AddEasyCaching(options =>
            {
                // use memory cache with your own configuration
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100,

                        // below two settings are added in v0.8.0
                        // enable deep clone when reading object from cache or not, default value is true.
                        EnableReadDeepClone = true,
                        // enable deep clone when writing object to cache or not, default valuee is false.
                        EnableWriteDeepClone = false,
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 120;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, localCacheName);

                //Important step for Redis Caching
                options.UseCSRedis(_cfg, remoteCacheName, "Redis");

                // combine local and distributed
                options.UseHybrid(config =>
                {
                    config.TopicName = topicName;
                    config.EnableLogging = true;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = localCacheName;
                    // specify the distributed cache provider name after v0.5.4
                    config.DistributedCacheProviderName = remoteCacheName;
                }, hyBridCacheName)
                // use csredis bus
                .WithCSRedisBus(busConf =>
                {
                    busConf.ConnectionStrings = _redisConfig.dbconfig.ConnectionStrings.ToList<string>();
                });

                //options.WithJson();
            });

            _services.ConfigureCastleInterceptor(options =>
            {
                options.CacheProviderName = hyBridCacheName;
            });
        }

        /// <summary>
        /// 注册跨域组件
        /// </summary>
        public virtual void AddCors()
        {
            _services.AddCors(options =>
            {
                var _corsHosts = _cfg.GetValue<string>("CorsHosts")?.Split(",", StringSplitOptions.RemoveEmptyEntries);
                options.AddPolicy(_serviceInfo.CorsPolicy, policy =>
                {
                    policy.WithOrigins(_corsHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        /// <summary>
        /// 注册swagger组件
        /// </summary>
        public virtual void AddSwaggerGen()
        {
            var openApiInfo = new OpenApiInfo { Title = _serviceInfo.ShortName, Version = _serviceInfo.Version };

            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(openApiInfo.Version, openApiInfo);

                // 采用bearer token认证
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                //设置全局认证
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{_serviceInfo.AssemblyName}.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{_serviceInfo.AssemblyName.Replace("WebApi", "Application")}.xml"));
                // Adds fluent validation rules to swagger
                c.AddFluentValidationRules();
            });
        }

        /// <summary>
        /// 注册健康监测组件
        /// </summary>
        public virtual void AddHealthChecks()
        {
            var redisString = _redisConfig.dbconfig.ConnectionStrings[0].Replace(",prefix=", string.Empty).Replace(",poolsize=50", string.Empty);
            _services.AddHealthChecks()
                     .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
                     //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
                     .AddMySql(_mysqlConfig.WriteDbConnectionString)
                     .AddMongoDb(_mongoConfig.ConnectionStrings)
                     .AddRabbitMQ(x =>
                     {
                         return
                         RabbitMqConnection.GetInstance(x.GetService<IOptionsSnapshot<RabbitMqConfig>>()
                             , x.GetService<ILogger<dynamic>>()
                         ).Connection;
                     })
                     //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
                     .AddRedis(redisString);
        }

        /// <summary>
        /// 注册CAP组件(实现事件总线及最终一致性（分布式事务）的一个开源的组件)
        /// </summary>
        /// <param name="tableNamePrefix">cap表面前缀</param>
        /// <param name="groupName">群组名子</param>
        /// <param name="func">回调函数</param>
        protected virtual void AddEventBusSubscribers(string tableNamePrefix, string groupName, Action<IServiceCollection> func)
        {
            _services.AddCap(x =>
            {
                //如果你使用的 EF 进行数据操作，你需要添加如下配置：
                //可选项，你不需要再次配置 x.UseSqlServer 了
                x.UseEntityFramework<AdncDbContext>(option =>
                {
                    option.TableNamePrefix = tableNamePrefix;
                });
                //CAP支持 RabbitMQ、Kafka、AzureServiceBus 等作为MQ，根据使用选择配置：
                x.UseRabbitMQ(option =>
                {
                    option.HostName = _rabbitMqConfig.HostName;
                    option.VirtualHost = _rabbitMqConfig.VirtualHost;
                    option.Port = _rabbitMqConfig.Port;
                    option.UserName = _rabbitMqConfig.UserName;
                    option.Password = _rabbitMqConfig.Password;
                });
                x.Version = _serviceInfo.Version;
                //默认值：cap.queue.{程序集名称},在 RabbitMQ 中映射到 Queue Names。
                x.DefaultGroup = groupName;
                //默认值：60 秒,重试 & 间隔
                //在默认情况下，重试将在发送和消费消息失败的 4分钟后 开始，这是为了避免设置消息状态延迟导致可能出现的问题。
                //发送和消费消息的过程中失败会立即重试 3 次，在 3 次以后将进入重试轮询，此时 FailedRetryInterval 配置才会生效。
                x.FailedRetryInterval = 60;
                //默认值：50,重试的最大次数。当达到此设置值时，将不会再继续重试，通过改变此参数来设置重试的最大次数。
                x.FailedRetryCount = 50;
                //默认值：NULL,重试阈值的失败回调。当重试达到 FailedRetryCount 设置的值的时候，将调用此 Action 回调
                //，你可以通过指定此回调来接收失败达到最大的通知，以做出人工介入。例如发送邮件或者短信。
                x.FailedThresholdCallback = (failed) =>
                {
                    //todo
                };
                //默认值：24*3600 秒（1天后),成功消息的过期时间（秒）。 
                //当消息发送或者消费成功时候，在时间达到 SucceedMessageExpiredAfter 秒时候将会从 Persistent 中删除，你可以通过指定此值来设置过期的时间。
                x.SucceedMessageExpiredAfter = 24 * 3600;
                //默认值：1,消费者线程并行处理消息的线程数，当这个值大于1时，将不能保证消息执行的顺序。
                x.ConsumerThreadCount = 1;
                x.UseDashboard(x =>
                {
                    x.PathMatch = $"/{_serviceInfo.ShortName}/cap";
                    x.Authorization = new IDashboardAuthorizationFilter[] {
                        new LocalRequestsOnlyAuthorizationFilter()
                        ,
                        new CapDashboardAuthorizationFilter()
                    };
                });
                //必须是生产环境才注册cap服务到consul
                if ((_env.IsProduction() || _env.IsStaging()))
                {
                    x.UseDiscovery();
                }
            });
            func?.Invoke(_services);
        }

        /// <summary>
        /// 注册Rpc服务(跨服务之间的同步通讯)
        /// </summary>
        /// <typeparam name="TRpcService">Rpc服务接口</typeparam>
        /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
        /// <param name="policies">Polly策略</param>
        /// <param name="token">Token，可空</param>
        protected virtual void AddRpcService<TRpcService>(string serviceName
        , List<IAsyncPolicy<HttpResponseMessage>> policies
        , Func<Task<string>> token = null
        ) where TRpcService : class, IRpcService
        {

            var prefix = serviceName.Substring(0, 7);
            bool isConsulAdderss = (prefix == "http://" || prefix == "https:/") ? false : true;

            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(SystemTextJsonHelper.GetAdncDefaultOptions()));
            //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
            var clientbuilder = _services.AddRefitClient<TRpcService>(refitSettings)
                                         .SetHandlerLifetime(TimeSpan.FromMinutes(2));
            //从consul获取地址
            if (isConsulAdderss)
            {
                clientbuilder.ConfigureHttpClient(c => c.BaseAddress = new Uri($"http://{serviceName}"))
                             .AddHttpMessageHandler(() =>
                             {
                                 return new ConsulDiscoveryDelegatingHandler(_consulConfig.ConsulUrl, token);
                             });
            }
            else
            {
                clientbuilder.ConfigureHttpClient((options) =>
                {
                    options.BaseAddress = new Uri(serviceName);
                });
            }

            //添加polly相关策略
            if (policies != null && policies.Any())
            {
                foreach (var policy in policies)
                    clientbuilder.AddPolicyHandler(policy);
            }
        }

        /// <summary>
        /// 生成默认的Polly策略
        /// </summary>
        /// <returns></returns>
        protected virtual List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultRefitPolicies()
        {
            //重试策略
            var retryPolicy = Policy.Handle<ApiException>()
                                    .OrResult<HttpResponseMessage>(response => response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                                    .WaitAndRetryAsync(new[]
                                    {
                                        TimeSpan.FromSeconds(1),
                                        TimeSpan.FromSeconds(5),
                                        TimeSpan.FromSeconds(10)
                                    });
            //超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);
            //隔离策略
            var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);
            //回退策略
            //断路策略
            var circuitBreakerPolicy = Policy.Handle<ApiException>()
                                     .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

            return new List<IAsyncPolicy<HttpResponseMessage>>()
            {
                //timeoutPolicy
            };
        }
    }
}
