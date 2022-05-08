using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Whse.WebApi.Registrar;

public sealed class WhseWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    private readonly Action<AutoMapper.IMapperConfigurationExpression> _createMaper = maperConfig =>
        {
            maperConfig.CreateMap<ProductSearchRequest, ProductSearchListDto>();
            maperConfig.CreateMap<ProductDto, ProductReply>();
        };

    public WhseWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddGrpcServer(_createMaper);
    }
}