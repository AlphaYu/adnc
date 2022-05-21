namespace Adnc.Shared.Rpc;

public class RpcConsts
{
    public static readonly long ProdunctStatusId = 1600000008500;
    public static readonly long OrderStatusId = 1600000008600;

    public const string Direct = "direct";
    public const string Consul = "consul";
    public const string CoreDns = "coredns";

    public readonly static string UsrService = "adnc.usr.api";
    public readonly static string MaintService = "adnc.maint.api";
    public readonly static string CusService = "adnc.cus.api";
    public readonly static string OrdService = "adnc.ord.api";
    public readonly static string WhseService = "adnc.whse.api";
}