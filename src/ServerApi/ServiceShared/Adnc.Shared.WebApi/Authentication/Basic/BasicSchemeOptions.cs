namespace Adnc.Shared.WebApi.Authentication.Basic;

public class BasicSchemeOptions : AuthenticationSchemeOptions
{
    public BasicSchemeOptions()
    {
        Events = new BasicEvents();
    }

    public new BasicEvents Events
    {
        get { return base.Events is null ? new BasicEvents() : (BasicEvents)base.Events; }
        set { base.Events = value; }
    }
}
