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
using DotNetCore.CAP.Dashboard;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Swashbuckle.AspNetCore.Swagger;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using Microsoft.AspNetCore.Authentication;
using Polly.Timeout;
using Microsoft.Extensions.Caching.Memory;
using Adnc.Infra.EventBus.RabbitMq;
using Adnc.Infra.Consul;
using Adnc.Infra.Consul.Consumer;
using Adnc.Infra.EfCore;
using Adnc.Infra.Mongo;
using Adnc.Infra.Mongo.Extensions;
using Adnc.Infra.Mongo.Configuration;
using Adnc.Application.Shared.RpcServices;
using Adnc.Infra.Common.Helper;
using Adnc.WebApi.Shared.Extensions;

namespace Adnc.WebApi.Shared
{
    public class SharedServicesRegistration
    {
        protected readonly IConfiguration _configuration;
        protected readonly IServiceCollection _services;
        protected readonly IWebHostEnvironment _environment;
        protected readonly ServiceInfo _serviceInfo;

        /// <summary>
        /// 服务注册与系统配置
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="environment"><see cref="IWebHostEnvironment"/></param>
        /// <param name="serviceInfo"><see cref="ServiceInfo"/></param>
        public SharedServicesRegistration(IConfiguration configuration
            , IServiceCollection services
            , IWebHostEnvironment environment
            , ServiceInfo serviceInfo)
        {
            _configuration = configuration;
            _environment = environment;
            _services = services;
            _serviceInfo = serviceInfo;
        }

        /// <summary>
        /// 注册配置类到IOC容器
        /// </summary>
        public virtual void Configure()
        {
            _services.Configure<JWTConfig>(_configuration.GetJWTSection());
            _services.Configure<MongoConfig>(_configuration.GetMongoDbSection());
            _services.Configure<MysqlConfig>(_configuration.GetMysqlSection());
            _services.Configure<RabbitMqConfig>(_configuration.GetRabbitMqSection());
            _services.Configure<ConsulConfig>(_configuration.GetConsulSection());
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
            var mysqlConfig = _configuration.GetMysqlSection().Get<MysqlConfig>();
            _services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(mysqlConfig.ConnectionString, mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
                    mySqlOptions.MinBatchSize(2);
                    mySqlOptions.CommandTimeout(10);
                    mySqlOptions.MigrationsAssembly(_serviceInfo.AssemblyName.Replace("WebApi", "Migrations"));
                    mySqlOptions.CharSet(CharSet.Utf8Mb4);
                });

                if (_environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }

                //替换默认查询sql生成器,如果通过mycat中间件实现读写分离需要替换默认SQL工厂。
                //options.ReplaceService<IQuerySqlGeneratorFactory, AdncMySqlQuerySqlGeneratorFactory>();
            });
        }

        /// <summary>
        /// 注册MongdbContext
        /// </summary>
        public virtual void AddMongoContext()
        {
            var mongoConfig = _configuration.GetMongoDbSection().Get<MongoConfig>();
            _services.AddMongo<MongoContext>(options =>
            {
                options.ConnectionString = mongoConfig.ConnectionString;
                options.PluralizeCollectionNames = mongoConfig.PluralizeCollectionNames;
                options.CollectionNamingConvention = (NamingConvention)mongoConfig.CollectionNamingConvention;
            });
        }

        /// <summary>
        /// 注册Jwt认证组件
        /// </summary>
        public virtual void AddJWTAuthentication()
        {
            var jwtConfig = _configuration.GetJWTSection().Get<JWTConfig>();

            //认证配置
            _services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer((Action<JwtBearerOptions>)(options =>
            {
                //验证的一些设置，比如是否验证发布者，订阅者，密钥，以及生命时间等等
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey)),
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkew)
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
                        var userContext = context.HttpContext.RequestServices.GetService<Application.Shared.IUserContext>();
                        var claims = context.Principal.Claims;
                        userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
                        userContext.Account = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                        //userContext.Name = claims.First(x => x.Type == ClaimTypes.Name).Value;
                        //userContext.Email = claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                        //string[] roleIds = claims.First(x => x.Type == ClaimTypes.Role).Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
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
            }));

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
            where THandler : PermissionHandler
        {
            _services.AddAuthorization(options =>
            {
                options.AddPolicy(Permission.Policy, policy => policy.Requirements.Add(new PermissionRequirement()));
            });
            _services.AddScoped<IAuthorizationHandler, THandler>();
        }

        /// <summary>
        /// 注册跨域组件
        /// </summary>
        public virtual void AddCors()
        {
            _services.AddCors(options =>
            {
                var _corsHosts = _configuration.GetAllowCorsHosts().Split(",", StringSplitOptions.RemoveEmptyEntries);
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
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{_serviceInfo.AssemblyName.Replace("WebApi", "Application.Contracts")}.xml"));
                // Adds fluent validation rules to swagger
                c.AddFluentValidationRules();
            });
        }

        /// <summary>
        /// 注册健康监测组件
        /// </summary>
        public virtual void AddHealthChecks()
        {
            var mysqlConfig = _configuration.GetMysqlSection().Get<MysqlConfig>();
            var mongoConfig = _configuration.GetMongoDbSection().Get<MongoConfig>();
            var redisConfig = _configuration.GetRedisSection().Get<RedisConfig>();
            _services.AddHealthChecks()
                     //.AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
                     //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
                     .AddMySql(mysqlConfig.ConnectionString)
                     .AddMongoDb(mongoConfig.ConnectionString)
                     .AddRabbitMQ(x =>
                     {
                         return
                         RabbitMqConnection.GetInstance(x.GetService<IOptionsSnapshot<RabbitMqConfig>>()
                             , x.GetService<ILogger<dynamic>>()
                         ).Connection;
                     })
                     //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
                    .AddRedis(redisConfig.dbconfig.ConnectionString);
        }

        /// <summary>
        /// 注册CAP组件的订阅者(实现事件总线及最终一致性（分布式事务）的一个开源的组件)
        /// </summary>
        /// <param name="tableNamePrefix">cap表面前缀</param>
        /// <param name="groupName">群组名子</param>
        /// <param name="func">回调函数</param>
        public virtual void AddEventBusSubscribers(string tableNamePrefix, string groupName, Action<IServiceCollection> func = null)
        {
            func?.Invoke(_services);

            var rabbitMqConfig = _configuration.GetRabbitMqSection().Get<RabbitMqConfig>();
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
                    option.HostName = rabbitMqConfig.HostName;
                    option.VirtualHost = rabbitMqConfig.VirtualHost;
                    option.Port = rabbitMqConfig.Port;
                    option.UserName = rabbitMqConfig.UserName;
                    option.Password = rabbitMqConfig.Password;
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
                if ((_environment.IsProduction() || _environment.IsStaging()))
                {
                    x.UseDiscovery();
                }
            });
        }

        /// <summary>
        /// 注册Rpc服务(跨微服务之间的同步通讯)
        /// </summary>
        /// <typeparam name="TRpcService">Rpc服务接口</typeparam>
        /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
        /// <param name="policies">Polly策略</param>
        public virtual void AddRpcService<TRpcService>(string serviceName
        , List<IAsyncPolicy<HttpResponseMessage>> policies
        ) where TRpcService : class, IRpcService
        {
            var prefix = serviceName.Substring(0, 7);
            bool isConsulAdderss = (prefix == "http://" || prefix == "https:/") ? false : true;

            var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(SystemTextJsonHelper.GetAdncDefaultOptions()));
            //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
            var clientbuilder = _services.AddRefitClient<TRpcService>(refitSettings)
                                         .SetHandlerLifetime(TimeSpan.FromMinutes(2));
            //如果参数是服务名字，那么需要从consul获取地址
            if (isConsulAdderss)
                clientbuilder.ConfigureHttpClient(client => client.BaseAddress = new Uri($"http://{serviceName}"))
                             .AddHttpMessageHandler<ConsulDiscoverDelegatingHandler>();
            else
                clientbuilder.ConfigureHttpClient(client => client.BaseAddress = new Uri(serviceName))
                             .AddHttpMessageHandler<SimpleDiscoveryDelegatingHandler>();

            //添加polly相关策略
            policies?.ForEach(policy => clientbuilder.AddPolicyHandler(policy));
        }

        /// <summary>
        /// 生成默认的Polly策略
        /// </summary>
        /// <returns></returns>
        public virtual List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultRefitPolicies()
        {
            //隔离策略
            //var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);

            //回退策略
            //回退也称服务降级，用来指定发生故障时的备用方案。
            //目前用不上
            //var fallbackPolicy = Policy<string>.Handle<HttpRequestException>().FallbackAsync("substitute data");

            //缓存策略
            //缓存策略无效
            //https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory?WT.mc_id=-blog-scottha#user-content-use-case-cachep
            //var cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            //var cacheProvider = new MemoryCacheProvider(cache);
            //var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(cacheProvider, TimeSpan.FromSeconds(100));

            //重试策略,超时或者API返回>500的错误,重试3次。
            //重试次数会统计到失败次数
            var retryPolicy = Policy.Handle<TimeoutRejectedException>()
                                    .OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500)
                                    .WaitAndRetryAsync(new[]
                                    {
                                        TimeSpan.FromSeconds(1),
                                        TimeSpan.FromSeconds(2),
                                        TimeSpan.FromSeconds(4)
                                    });
            //超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(_environment.IsDevelopment() ? 10 : 3);

            //熔断策略
            //如下，如果我们的业务代码连续失败50次，就触发熔断(onBreak),就不会再调用我们的业务代码，而是直接抛出BrokenCircuitException异常。
            //当熔断时间10分钟后(durationOfBreak)，切换为HalfOpen状态，触发onHalfOpen事件，此时会再调用一次我们的业务代码
            //，如果调用成功，则触发onReset事件，并解除熔断，恢复初始状态，否则立即切回熔断状态。
            var circuitBreakerPolicy = Policy.Handle<Exception>()
                                             .CircuitBreakerAsync
                                             (
                                                 // 熔断前允许出现几次错误
                                                 exceptionsAllowedBeforeBreaking: 10
                                                 ,
                                                 // 熔断时间,熔断10分钟
                                                 durationOfBreak: TimeSpan.FromMinutes(3)
                                                 ,
                                                 // 熔断时触发
                                                 onBreak: (ex, breakDelay) =>
                                                 {
                                                     //todo
                                                     var e = ex;
                                                     var delay = breakDelay;
                                                 }
                                                 ,
                                                 //熔断恢复时触发
                                                 onReset: () =>
                                                 {
                                                     //todo
                                                 }
                                                 ,
                                                 //在熔断时间到了之后触发
                                                 onHalfOpen: () =>
                                                 {
                                                     //todo
                                                 }
                                             );

            return new List<IAsyncPolicy<HttpResponseMessage>>()
            {
                retryPolicy
               ,timeoutPolicy
               ,circuitBreakerPolicy.AsAsyncPolicy<HttpResponseMessage>()
            };
        }

        /// <summary>
        /// 默认获取Token的方法
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> GetTokenDefaultFunc()
        {
            return await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
        }
    }
}
