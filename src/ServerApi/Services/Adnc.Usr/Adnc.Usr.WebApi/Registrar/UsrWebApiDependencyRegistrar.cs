using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Usr.WebApi.Registrar;

public sealed class UsrWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    private readonly Action<AutoMapper.IMapperConfigurationExpression> _createMaper = maperConfig =>
    {
        maperConfig.CreateMap<LoginRequest, UserLoginDto>();
        maperConfig.CreateMap<DeptTreeDto, DeptReply>();
    };

    public UsrWebApiDependencyRegistrar(IServiceCollection services) : base(services) { }

    public override void AddAdnc()
    {
        AddWebApiDefault<PermissionHandlerLocal>();
        AddGrpcServer(_createMaper);
    }
}