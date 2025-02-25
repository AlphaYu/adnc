using Adnc.Infra.Helper.Internal;
using Adnc.Infra.Helper.Internal.Encrypt;

namespace Adnc.Infra.Helper;

public static class InfraHelper
{
    private readonly static EncryptProivder _encypt = new();
    private readonly static HashConsistentGenerater _hashConsistentGenerater = new();
    private readonly static Accessor _accessor = new();

    static InfraHelper()
    {
    }

    public static EncryptProivder Encrypt => _encypt;

    public static HashConsistentGenerater HashConsistent => _hashConsistentGenerater;

    public static Accessor Accessor => _accessor;
}