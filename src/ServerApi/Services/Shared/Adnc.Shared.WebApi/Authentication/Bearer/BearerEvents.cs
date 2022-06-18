using Adnc.Shared.Rpc.Handlers.Token;

namespace Adnc.Shared.WebApi.Authentication.Bearer;

public class BearerEvents
{
    public Func<string, Claim[]> OnTokenValidating { get; set; }
    public Func<BearerTokenValidatedContext, Task> OnTokenValidated { get; set; }
}
