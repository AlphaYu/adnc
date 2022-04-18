using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Cus.WebApi.Registrar;

public sealed class CustWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public CustWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
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