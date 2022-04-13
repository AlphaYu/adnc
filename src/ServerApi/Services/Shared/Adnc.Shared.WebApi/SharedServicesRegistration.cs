using Adnc.Infra.EventBus.RabbitMq;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Adnc.Shared.WebApi;

public class SharedServicesRegistration
{
    protected readonly IConfiguration _configuration;
    protected readonly IServiceCollection _services;
    protected readonly IHostEnvironment _environment;
    protected readonly IServiceInfo _serviceInfo;
    protected internal IEnumerable<Type> _schedulingJobs;

    /// <summary>
    /// 服务注册与系统配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="services"><see cref="IServiceInfo"/></param>
    /// <param name="environment"><see cref="IHostEnvironment"/></param>
    /// <param name="serviceInfo"><see cref="ServiceInfo"/></param>
    public SharedServicesRegistration(IConfiguration configuration
        , IServiceCollection services
        , IServiceInfo serviceInfo)
    {
        _configuration = configuration;
        _services = services;
        _serviceInfo = serviceInfo;
        _schedulingJobs = Enumerable.Empty<Type>();
    }

    /// <summary>
    /// 注册配置类到IOC容器
    /// </summary>
    public virtual void Configure()
    {
        _services.Configure<JwtConfig>(_configuration.GetJWTSection());
        _services.Configure<MongoConfig>(_configuration.GetMongoDbSection());
        _services.Configure<MysqlConfig>(_configuration.GetMysqlSection());
        _services.Configure<RabbitMqConfig>(_configuration.GetRabbitMqSection());
        _services.Configure<ConsulConfig>(_configuration.GetConsulSection());
        _services.Configure<ThreadPoolSettings>(_configuration.GetThreadPoolSettingsSection());
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
        _services.Configure<ApiBehaviorOptions>(options =>
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
        var jwtConfig = _configuration.GetJWTSection().Get<JwtConfig>();

        _services.AddAuthentication(HybridDefaults.AuthenticationScheme)
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
        _services.AddScoped<IAuthorizationHandler, THandler>();
        _services.AddAuthorization(options =>
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
                    Array.Empty<string>()
                }
            });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{_serviceInfo.AssemblyName}.xml"));
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{_serviceInfo.AssemblyName.Replace("WebApi", "Application.Contracts")}.xml"));
        });

        _services.AddFluentValidationRulesToSwagger();
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
                         RabbitMqConnection.GetInstance(x.GetService<IOptionsMonitor<RabbitMqConfig>>()
                             , x.GetService<ILogger<dynamic>>()
                         ).Connection;
                     })
                    //.AddUrlGroup(new Uri("https://localhost:5001/weatherforecast"), "index endpoint")
                    //await HttpContextUtility.GetCurrentHttpContext().GetTokenAsync("access_token");
                    .AddRedis(redisConfig.dbconfig.ConnectionString);
    }
}