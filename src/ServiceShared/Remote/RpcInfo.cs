namespace Adnc.Shared.Remote;

public sealed class RpcInfo
{
    public List<AddressNode> Address { get; set; } = [];

    public PollyNode Polly { get; set; } = new();

    public class AddressNode
    {
        public string Service { get; set; } = string.Empty;
        public string Direct { get; set; } = string.Empty;
        public string Consul { get; set; } = string.Empty;
        public string CoreDns { get; set; } = string.Empty;
    }

    public class PollyNode
    {
        public bool Enable { get; set; }
    }
}
