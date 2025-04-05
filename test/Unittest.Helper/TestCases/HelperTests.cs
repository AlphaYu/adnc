using Adnc.Infra.Helper;
using Xunit.Abstractions;

namespace Adnc.Infra.Unittest.Helper.TestCases;

public class HelperTests
{
    private readonly ITestOutputHelper _output;

    public HelperTests(ITestOutputHelper output)
    {
        _output = output;
    }

    /// <summary>
    /// DES加密解密测试
    /// </summary>
    [Fact]
    public void TestAes()
    {
        var aesKey = InfraHelper.Encrypt.CreateAesKey();
        var text = "MP1S0RHE";

        var ciphertext = InfraHelper.Encrypt.AESEncrypt(text, aesKey.Key);
        var plaintext = InfraHelper.Encrypt.AESDecrypt(ciphertext, aesKey.Key);

        _output.WriteLine($"key：{aesKey.Key}，resgisrationcode：{ciphertext}");

        Assert.Equal(text, plaintext);
    }
}
