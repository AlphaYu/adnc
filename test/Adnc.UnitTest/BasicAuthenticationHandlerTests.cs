using Adnc.Shared.Rpc.Handlers.Token;

namespace Adnc.UnitTest.BasicAuthentication;

public class BasicAuthenticationHandlerTests
{
    public BasicAuthenticationHandlerTests()
    {
    }

    [Fact]
    public void PackAndUnPackBase64()
    {
        var token = BasicTokenGeneratorExtension.PackToBase64(null, "usr");
        var (isSuccessful, userName, appId) = BasicTokenGeneratorExtension.UnPackFromBase64(null, token);
        Assert.Equal("usr", userName);
        Assert.NotEmpty(appId);
        Assert.True(isSuccessful);
    }
}