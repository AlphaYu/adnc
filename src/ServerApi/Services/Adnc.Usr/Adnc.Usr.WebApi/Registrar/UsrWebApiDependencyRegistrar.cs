using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Usr.WebApi.Registrar;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public UsrWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        Services.AddHttpContextAccessor();
        Services.AddMemoryCache();
        Configure();
        AddControllers();
        AddAuthentication();
        AddAuthorization<PermissionHandlerLocal>();
        AddCors();
        AddSwaggerGen();
        AddHealthChecks();
        AddMiniProfiler();
        AddApplicationServices();
    }
}