namespace Adnc.Shared.Rpc;

//public class RpcAddressInfo
//{
//    public List<AddressNode> RootAddresses { get; set; } = new();

//    private static readonly Lazy<RpcAddressInfo> s_Instance = new(() =>
//    {
//        var filePath = @$"{AppContext.BaseDirectory}/rpcserviceinfos.json";
//        var json = File.ReadAllText(filePath);
//        var result = JsonSerializer.Deserialize<RpcAddressInfo>(json);
//        if (result is null)
//            throw new NotImplementedException();
//        return result;
//    });

//    public static AddressNode? GetAddressNode(string service) => s_Instance.Value.RootAddresses.FirstOrDefault(x => x.Service.EqualsIgnoreCase(service));
//}

public class AddressNode
{
    public const string Name = "RpcAddressInfo";
    public string Service { get; set; } = string.Empty;
    public string Direct { get; set; } = string.Empty;
    public string Consul { get; set; } = string.Empty;
    public string CoreDns { get; set; } = string.Empty;
}
