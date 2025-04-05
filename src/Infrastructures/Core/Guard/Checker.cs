namespace Adnc.Infra.Core.Guard;

public static class Checker
{
    static Checker()
    {
    }

    public static ArgumentChecker Argument => ArgumentChecker.Instance;

    public static VariableChecker Variable => VariableChecker.Instance;
}
