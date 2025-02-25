namespace Adnc.Shared.WebApi.Authentication.Bearer;

public class BearerEvents
{
    public Func<BearerTokenValidatedContext, Task> OnTokenValidated { get; set; } = default!;
}
