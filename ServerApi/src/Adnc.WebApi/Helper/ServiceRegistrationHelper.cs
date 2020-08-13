using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EasyCaching.InMemory;
using EasyCaching.Serialization.Json;
using Adnc.Application;
using Microsoft.EntityFrameworkCore;
using Adnc.Infr.EfCore;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Adnc.Infr.EfCore.Interceptors;
using Adnc.Core;
using Adnc.Infr.Mongo;
using Adnc.Infr.Mongo.Extensions;
using Adnc.Infr.Mongo.Configuration;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Adnc.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Adnc.Infr.Consul;

namespace Adnc.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceCollection _services;
        private readonly JWTConfig _jwtConfig;
        private readonly MongoConfig _mongoConfig;
        private readonly MysqlConfig _mysqlConfig;
        private readonly ConsulOption _consulOption;

        private static volatile ServiceRegistrationHelper _uniqueInstance;
        private static readonly object _lockObject = new object();


        public static ServiceRegistrationHelper GetInstance(IConfiguration configuration, IServiceCollection services)
        {
            if (_uniqueInstance == null)
            {
                lock (_lockObject)
                {
                    if (_uniqueInstance == null)
                    {
                        _uniqueInstance = new ServiceRegistrationHelper(configuration, services);
                    }
                }
            }
            return _uniqueInstance;
        }

        private ServiceRegistrationHelper(IConfiguration configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;
            
            Configure();

            _jwtConfig = _configuration.GetSection("JWT").Get<JWTConfig>();
            _mongoConfig = _configuration.GetSection("MongoDb").Get<MongoConfig>();
            _mysqlConfig = _configuration.GetSection("Mysql").Get<MysqlConfig>();
        }

        private void Configure()
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
        }

        public void AddCaching()
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
                }, EasyCachingConsts.LocalCaching);

                //Important step for Redis Caching
                options.UseCSRedis(_configuration, EasyCachingConsts.RemoteCaching, "Redis");

                // combine local and distributed
                options.UseHybrid(config =>
                {
                    config.TopicName = EasyCachingConsts.TopicName;
                    config.EnableLogging = true;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = EasyCachingConsts.LocalCaching;
                    // specify the distributed cache provider name after v0.5.4
                    config.DistributedCacheProviderName = EasyCachingConsts.RemoteCaching;
                }, EasyCachingConsts.HybridCaching)
                // use csredis bus
                .WithCSRedisBus(busConf =>
                {
                    busConf.ConnectionStrings = _configuration.GetSection("Redis:dbConfig:ConnectionStrings").Get<List<string>>();
                });

                //options.WithJson();
            });

            _services.ConfigureCastleInterceptor(options =>
            {
                options.CacheProviderName = EasyCachingConsts.HybridCaching;
            });
        }

        public void AddEfCoreContext()
        {
            _services.AddScoped<IUnitOfWork, UnitOfWork<AdncDbContext>>();
            _services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(_mysqlConfig.WriteDbConnectionString, mySqlOptions => 
                { 
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
                    mySqlOptions.MinBatchSize(2);
                });
                options.AddInterceptors(new CustomCommandInterceptor());
            });

        }

        public void AddMongoContext()
        {
            _services.AddMongo<MongoContext>(options =>
            {
                options.ConnectionString = _mongoConfig.ConnectionStrings;
                options.PluralizeCollectionNames = _mongoConfig.PluralizeCollectionNames;
                options.CollectionNamingConvention = (NamingConvention)_mongoConfig.CollectionNamingConvention;
            });
        }

        public void AddControllers()
        {
            _services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                        options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
                        //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
        }

        public void AddJWTAuthentication()
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

        public void AddAuthorization()
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
            _services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        }

        public void AddCors(string corsPolicy)
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

        public void AddSwaggerGen()
        {
            _services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.5.0", new OpenApiInfo { Title = "adnc-api-sys", Version = "v0.5.0" });

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

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Adnc.Application.xml"));
            });
        }

        public void AddHealthChecks()
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
    }
}
