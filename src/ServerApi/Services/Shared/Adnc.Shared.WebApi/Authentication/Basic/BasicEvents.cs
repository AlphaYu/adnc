using Adnc.Shared.Rpc.Handlers.Token;

namespace Adnc.Shared.WebApi.Authentication.Basic;

public class BasicEvents
{
    public Func<BasicTokenValidatedContext, Task> OnTokenValidated { get; set; }

    //public virtual Task TokenValidated(BasicTokenValidatedContext context) => OnTokenValidated(context);
}
