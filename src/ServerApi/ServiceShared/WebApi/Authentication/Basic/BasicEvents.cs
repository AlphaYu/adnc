namespace Adnc.Shared.WebApi.Authentication.Basic;

public class BasicEvents
{
    public Func<BasicTokenValidatedContext, Task> OnTokenValidated { get; set; } = default!;

    //public virtual Task TokenValidated(BasicTokenValidatedContext context) => OnTokenValidated(context);
}
