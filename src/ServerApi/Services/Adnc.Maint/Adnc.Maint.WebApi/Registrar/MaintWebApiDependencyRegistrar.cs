using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Maint.WebApi.Registrar;

public sealed class MaintWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    private readonly Action<AutoMapper.IMapperConfigurationExpression> _createMaper = maperConfig =>
        {
            maperConfig.CreateMap<DictDto, DictReply>();
        };

    public MaintWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddGrpcServer(_createMaper);
    }
}