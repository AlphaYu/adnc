﻿using StackExchange.Profiling.Storage;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Adnc.Shared.WebApi.Registrar;

public abstract class AbstractWebApiDependencyRegistrar : IDependencyRegistrar
{
    public string Name => "webapi";
    protected IConfiguration Configuration;
    protected readonly IServiceCollection Services;
    protected readonly IHostEnvironment HostEnvironment;
    protected readonly IServiceInfo ServiceInfo;

    /// <summary>
    /// 服务注册与系统配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="services"><see cref="IServiceInfo"/></param>
    /// <param name="environment"><see cref="IHostEnvironment"/></param>
    /// <param name="serviceInfo"><see cref="ServiceInfo"/></param>
    protected AbstractWebApiDependencyRegistrar(IServiceCollection services)
    {
        Services = services;
        Configuration = services.GetConfiguration();
        ServiceInfo = services.GetServiceInfo();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract void AddAdnc();

    /// <summary>
    /// 注册配置类到IOC容器
    /// </summary>
    public virtual void Configure()
    {
        Services.Configure<JwtConfig>(Configuration.GetJWTSection());
        Services.Configure<MongoConfig>(Configuration.GetMongoDbSection());
        Services.Configure<MysqlConfig>(Configuration.GetMysqlSection());
        Services.Configure<RabbitMqConfig>(Configuration.GetRabbitMqSection());
        Services.Configure<ConsulConfig>(Configuration.GetConsulSection());
        Services.Configure<ThreadPoolSettings>(Configuration.GetThreadPoolSettingsSection());
    }

    /// <summary>
    /// Controllers 注册
    /// Sytem.Text.Json 配置
    /// FluentValidation 注册
    /// ApiBehaviorOptions 配置
    /// </summary>
    public virtual void AddControllers()
    {
        Services.AddControllers(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            options.JsonSerializerOptions.Converters.Add(new DateTimeNullableConverter());
            options.JsonSerializerOptions.Encoder = SystemTextJson.GetAdncDefaultEncoder();
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
        Services.Configure<ApiBehaviorOptions>(options =>
        {
            //关闭自动验证
            //options.SuppressModelStateInvalidFilter = true;
            //格式化验证信息
            options.InvalidModelStateResponseFactory = (context) =>
            {
                var problemDetails = new ProblemDetails
                {
                    Detail = context.ModelState.GetValidationSummary("<br>"),
                    Title = "参数错误",
                    Status = (int)HttpStatusCode.BadRequest,
                    Type = "https://httpstatuses.com/400",
                    Instance = context.HttpContext.Request.Path
                };

                return new ObjectResult(problemDetails)
                {
                    StatusCode = problemDetails.Status
                };
            };
        });

        //add skyamp
        //_services.AddSkyApmExtensions().AddCaching();
    }

    /// <summary>
    /// 注册身份认证组件
    /// </summary>
    public virtual void AddAuthentication()
    {
        var jwtConfig = Configuration.GetJWTSection().Get<JwtConfig>();

        Services.AddAuthentication(HybridDefaults.AuthenticationScheme)
        .AddHybrid()
        .AddBasic()
        .AddJwtBearer(options =>
        {
            //校验配置
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SymmetricSecurityKey)),
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(jwtConfig.ClockSkew),
                //AudienceValidator = (m, n, z) =>m != null && m.FirstOrDefault().Equals(Const.ValidAudience)
            };
            //校验后事件
            options.Events = new JwtBearerEvents
            {
                //接受到消息时调用
                OnMessageReceived = context => Task.CompletedTask
                ,
                //在Token验证通过后调用
                OnTokenValidated = context =>
                {
                    var userContext = context.HttpContext.RequestServices.GetService<IUserContext>();
                    var claims = context.Principal.Claims;
                    userContext.Id = long.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
                    userContext.Account = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;
                    userContext.Name = claims.First(x => x.Type == JwtRegisteredClaimNames.Name).Value;
                    userContext.RemoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                    return Task.CompletedTask;
                }
                 ,
                //认证失败时调用
                OnAuthenticationFailed = context =>
                {
                    //如果是过期，在http heard中加入act参数
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        context.Response.Headers.Add("act", "expired");
                    return Task.CompletedTask;
                }
                ,
                //未授权时调用
                OnChallenge = context => Task.CompletedTask
            };
        });

        //因为获取声明的方式默认是走微软定义的一套映射方式
        //如果我们想要走JWT映射声明，那么我们需要将默认映射方式给移除掉
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }

    /// <summary>
    /// 注册授权组件
    /// PermissionHandlerRemote 跨服务授权
    /// PermissionHandlerLocal  本地授权,adnc.usr走本地授权，其他服务走Rpc授权
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public virtual void AddAuthorization<THandler>()
        where THandler : AbstractPermissionHandler
    {
        Services.AddScoped<IAuthorizationHandler, THandler>();
        Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizePolicy.Default, policy =>
            {
                policy.Requirements.Add(new PermissionRequirement());
            });
        });
    }

    /// <summary>
    /// 注册跨域组件
    /// </summary>
    public virtual void AddCors()
    {
        Services.AddCors(options =>
        {
            var _corsHosts = Configuration.GetAllowCorsHosts().Split(",", StringSplitOptions.RemoveEmptyEntries);
            options.AddPolicy(ServiceInfo.CorsPolicy, policy =>
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
        var openApiInfo = new OpenApiInfo { Title = ServiceInfo.ShortName, Version = ServiceInfo.Version };

        Services.AddSwaggerGen(c =>
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
                    Array.Empty<string>()
                }
            });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{ServiceInfo.AssemblyName}.xml"));
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{ServiceInfo.AssemblyName.Replace("WebApi", "Application.Contracts")}.xml"));
        });

        Services.AddFluentValidationRulesToSwagger();
    }

    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    public virtual void AddHealthChecks()
    {
        var mysqlConfig = Configuration.GetMysqlSection().Get<MysqlConfig>();
        var mongoConfig = Configuration.GetMongoDbSection().Get<MongoConfig>();
        var redisConfig = Configuration.GetRedisSection().Get<RedisConfig>();
        Services.AddHealthChecks()
                     //.AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 200, tags: new[] { "memory" })
                     //.AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running
                     .AddMySql(mysqlConfig.ConnectionString)
                     .AddMongoDb(mongoConfig.ConnectionString)
                     .AddRabbitMQ(x =>
                     {
                         return
                         Adnc.Infra.EventBus.RabbitMq.RabbitMqConnection.GetInstance(x.GetService<IOptionsMonitor<RabbitMqConfig>>()
                             , x.GetService<ILogger<dynamic>>()
                         ).Connection;
                     })
                    //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
                    //await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
                    .AddRedis(redisConfig.dbconfig.ConnectionString);
    }

    /// <summary>
    /// 注册 MiniProfiler 组件
    /// </summary>
    public virtual void AddMiniProfiler()
    {
        Services.AddMiniProfiler(options => options.RouteBasePath = "/usr/profiler").AddEntityFramework();
    }

    /// <summary>
    /// 注册Application层服务
    /// </summary>
    public virtual void AddApplicationServices()
    {
        var appAssembly = ServiceInfo.GetApplicationAssembly();
        if (appAssembly != null)
        {
            var modelType = appAssembly.GetTypes().FirstOrDefault(m => m.IsAssignableTo(typeof(IDependencyRegistrar)) && m.IsNotAbstractClass(true));
            if (modelType != null)
            {
                var adncServiceCollection = Activator.CreateInstance(modelType, Services) as IDependencyRegistrar;
                adncServiceCollection.AddAdnc();
            }
        }
    }
}