namespace Adnc.Infra.Helper.Internal.Encrypt;

public sealed partial class EncryptProivder
{
    internal EncryptProivder()
    {
    }

    private Random RandomInstance { get; } = new Random();
}
