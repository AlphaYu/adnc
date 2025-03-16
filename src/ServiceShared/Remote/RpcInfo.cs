namespace Adnc.Shared.Remote;

public sealed class RpcInfo
{
    public string Type { get; set; } = string.Empty;

    public List<AddressNode> Address { get; set; } = [];

    public class AddressNode
    {
        public string Service { get; set; } = string.Empty;
        public string Direct { get; set; } = string.Empty;
        public string Consul { get; set; } = string.Empty;
        public string CoreDns { get; set; } = string.Empty;
    }
}
