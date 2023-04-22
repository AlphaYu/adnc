namespace Adnc.Demo.Shared.Rpc.Grpc;

public static class GrpcClientConsts
{
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