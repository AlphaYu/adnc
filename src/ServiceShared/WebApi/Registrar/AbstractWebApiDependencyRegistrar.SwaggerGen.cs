namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// Registers Swagger components.
    /// </summary>
    protected virtual void AddSwaggerGen()
    {
        var openApiInfo = new OpenApiInfo { Title = ServiceInfo.ShortName, Version = ServiceInfo.Version };
        var startAssemblyName = ServiceInfo.StartAssembly.GetName().Name ?? throw new InvalidDataException($"{nameof(ServiceInfo.StartAssembly)} is null");
        //Services.AddEndpointsApiExplorer();
        Services
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc(openApiInfo.Version, openApiInfo);

                // Use bearer token authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                // Set global authentication
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

                var lastName = startAssemblyName.Split('.').Last();
                var apiLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName}.xml");
                var applicationContractsLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName.Replace($".{lastName}", ".Application.Contracts")}.xml");
                var applicationLayerXmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{startAssemblyName.Replace($".{lastName}", ".Application")}.xml");
                c.IncludeXmlComments(apiLayerXmlFilePath, true);
                if (File.Exists(applicationContractsLayerXmlFilePath))
                {
                    c.IncludeXmlComments(applicationContractsLayerXmlFilePath, true);
                }
                else if (File.Exists(applicationLayerXmlFilePath))
                {
                    c.IncludeXmlComments(applicationLayerXmlFilePath, true);
                }
            })
            .AddFluentValidationRulesToSwagger();
    }
}
