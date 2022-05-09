namespace Adnc.Infra.Core.Guard;

public interface IGuard
{
}

public class Guard : IGuard
{
    public static IGuard Checker { get; } = new Guard();

    private Guard()
    { }
}