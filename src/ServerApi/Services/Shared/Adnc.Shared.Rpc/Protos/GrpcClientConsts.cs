using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Adnc.Shared.Rpc.Grpc;

public static class GrpcClientConsts
{
    public static readonly long ProdunctStatusId = 1600000008500;

    public static readonly long OrderStatusId = 1600000008600;

    public static Empty Empty => new();

    public static Metadata BearerHeader => new()
    {
        { "Authorization", "Bearer" }
    };

    public static Metadata BasicHeader => new()
    {
        { "Authorization", "Basic" }
    };
}