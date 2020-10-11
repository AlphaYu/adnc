using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Refit;
using RefitConsul;
using EasyCaching.InMemory;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Common.Models;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Infr.Consul;
using Adnc.Core.Shared;

namespace Adnc.WebApi.Shared
{
    public abstract class SharedServicesRegistration
    {
        protected readonly IConfiguration _configuration;
        protected readonly IServiceCollection _services;
        protected readonly JWTConfig _jwtConfig;
        protected readonly MongoConfig _mongoConfig;
        protected readonly MysqlConfig _mysqlConfig;
        protected readonly RedisConfig _redisConfig;
        protected readonly RabbitMqConfig _rabbitMqConfig;
        protected readonly ConsulConfig _consulConfig;

        public SharedServicesRegistration(IConfiguration configuration, IServiceCollection services)
        {
            _services = services;
            _configuration = configuration;

            _jwtConfig = _configuration.GetSection("JWT").Get<JWTConfig>();
            _mongoConfig = _configuration.GetSection("MongoDb").Get<MongoConfig>();
            _mysqlConfig = _configuration.GetSection("Mysql").Get<MysqlConfig>();
            _redisConfig = _configuration.GetSection("Redis").Get<RedisConfig>();
            _rabbitMqConfig = _configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
            _consulConfig = _configuration.GetSection("Consul").Get<ConsulConfig>();
        }

        public JWTConfig GetJWTConfig()
        {
            return _jwtConfig;
        }

        public MongoConfig GetMongoConfig()
        {
            return _mongoConfig;
        }

        public MysqlConfig GetMysqlConfig()
        {
            return _mysqlConfig;
        }

        public RedisConfig GetRedisConfig()
        {
            return _redisConfig;
        }

        public RabbitMqConfig GetRabbitMqConfig()
        {
            return _rabbitMqConfig;
        }

        public ConsulConfig GetConsulConfig()
        {
            return _consulConfig;
        }

        public virtual void Configure()
        {
            // 获取客户端真实Ip
            //https://docs.microsoft.com/zh-cn/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.0#configuration-for-an-ipv4-address-represented-as-an-ipv6-address
            _services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            _services.Configure<JWTConfig>(_configuration.GetSection("JWT"));
            _services.Configure<MongoConfig>(_configuration.GetSection("MongoDb"));
            _services.Configure<MysqlConfig>(_configuration.GetSection("Mysql"));
            _services.Configure<RabbitMqConfig>(_configuration.GetSection("RabbitMq"));
        }

        public virtual void AddControllers()
        {
        }

        public virtual void AddMqHostedServices()
        {
        }

        public virtual void AddEfCoreContext()
        {
        }

        public virtual void AddMongoContext()
        {
        }

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
                        userContext.RemoteIpAddress = $"{ context.HttpContext.Connection.RemoteIpAddress}:{ context.HttpContext.Connection.RemotePort}";

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

        public virtual void AddAuthorization()
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
            _services.AddScoped<IAuthorizationHandler, PermissionHandlerRemote>();
        }

        public virtual void AddCaching(string localCacheName, string remoteCacheName, string hyBridCacheName, string topicName)
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
                options.UseCSRedis(_configuration, remoteCacheName, "Redis");

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

        public virtual void AddCors(string corsPolicy)
        {
            _services.AddCors(options =>
            {
                var _corsHosts = _configuration.GetValue<string>("CorsHosts")?.Split(",", StringSplitOptions.RemoveEmptyEntries);
                options.AddPolicy(corsPolicy, policy => 
                {
                    policy.WithOrigins(_corsHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        public virtual void AddSwaggerGen(OpenApiInfo openApiInfo,List<string> filepaths)
        {
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

                if (filepaths != null)
                {
                    foreach(var filepath in filepaths)
                    {
                        c.IncludeXmlComments(filepath);
                    }
                }
            });
        }

        public virtual void AddHealthChecks()
        {
            _services.AddHealthChecks()
                     .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
                     .AddMySql(_mysqlConfig.WriteDbConnectionString)
                     .AddMongoDb(_mongoConfig.ConnectionStrings);
                    //.AddRedis("localhost:10888,password=,defaultDatabase=1,defaultDatabase=10", "redis1");
                    //.AddCheck(name: "random", () =>
                    //{
                    //    return DateTime.UtcNow.Second % 3 == 0 ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
                    //})
                    //.AddAsyncCheck("Http", async () =>
                    //{
                    //    using (HttpClient client = new HttpClient())
                    //    {
                    //        try
                    //        {
                    //            var response = await client.GetAsync("http://localhost:5000/index.html");
                    //            if (!response.IsSuccessStatusCode)
                    //            {
                    //                throw new Exception("Url not responding with 200 OK");
                    //            }
                    //        }
                    //        catch (Exception)
                    //        {
                    //            return await Task.FromResult(HealthCheckResult.Unhealthy());
                    //        }
                    //    }
                    //    return await Task.FromResult(HealthCheckResult.Healthy());
                    //})

        }

        public virtual void AddRpcService<TRpcService>(string serviceName
            , List<IAsyncPolicy<HttpResponseMessage>> policies
            , Func<Task<string>> token
            )
            where TRpcService : class, IRpcService
        {

            var prefix = serviceName.Substring(0, 7);
            bool isConsulAdderss = (prefix == "http://" || prefix == "https:/") ? false : true;

            //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
            //var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer()
            var clientbuilder = _services.AddRefitClient<TRpcService>()                              
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
            if (policies!=null && policies.Any())
            {
                foreach (var policy in policies)
                    clientbuilder.AddPolicyHandler(policy);
            }
        }
    }
}
