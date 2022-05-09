namespace Adnc.Infra.Helper;

public class InfraHelper
{
    private InfraHelper()
    { }

    public static ISecurity Security => new Security();

    public static IHashGenerater Hash => new HashGenerater();
}