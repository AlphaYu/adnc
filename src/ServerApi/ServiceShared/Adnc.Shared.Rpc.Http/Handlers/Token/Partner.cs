namespace Adnc.Shared.Rpc.Http.Handlers.Token;

public class Partner
{
    public const string Name = "RpcPartners";
    public string UserName { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string SecurityKey { get; set; } = string.Empty;
}
