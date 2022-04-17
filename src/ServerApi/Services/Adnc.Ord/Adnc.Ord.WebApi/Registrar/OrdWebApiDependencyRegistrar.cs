using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Ord.WebApi.Registrar;

public sealed class OrdWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public OrdWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdncServices()
    {
        Services.AddHttpContextAccessor();
        Services.AddMemoryCache();
        Configure();
        AddControllers();
        AddAuthentication();
        AddAuthorization<PermissionHandlerRemote>();
        AddCors();
        AddSwaggerGen();
        AddHealthChecks();
        AddMiniProfiler();
        AddApplicationServices();
    }
}