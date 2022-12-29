﻿using Adnc.Shared.WebApi.Registrar;

namespace Adnc.Whse.WebApi;

public sealed class WhseWebApiDependencyRegistrar : AbstractWebApiDependencyRegistrar
{
    public WhseWebApiDependencyRegistrar(IServiceCollection services)
        : base(services)
    {
    }

    public WhseWebApiDependencyRegistrar(IApplicationBuilder app)
        : base(app)
    {
    }

    public override void AddAdnc()
    {
        AddWebApiDefault();

        var connectionString = Configuration.GetValue<string>("SqlServer:ConnectionString");
        AddHealthChecks(false, true, true, true).AddSqlServer(connectionString);

        Services.AddGrpc();
    }

    public override void UseAdnc()
    {
        UseWebApiDefault(endpointRoute: endpoint =>
        {
            endpoint.MapGrpcService<Grpc.WhseGrpcServer>();
        });
    }
}