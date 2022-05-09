using Adnc.Shared.Rpc.Grpc.Messages;
using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Whse.WebApi.Registrar;

public sealed class WhseWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public WhseWebApiDependencyRegistrar(IServiceCollection services) : base(services)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();
        AddGrpcServer();
    }
}