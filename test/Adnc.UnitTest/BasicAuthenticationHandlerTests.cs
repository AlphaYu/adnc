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
        var token = BasicTokenValidator.PackToBase64(BasicTokenValidator.InternalCaller);
        var (isSuccessful, userName, appId) = BasicTokenValidator.UnPackFromBase64(token);
        Assert.Equal(BasicTokenValidator.InternalCaller,userName);
        Assert.NotEmpty(appId);
        Assert.True(isSuccessful);
    }
}