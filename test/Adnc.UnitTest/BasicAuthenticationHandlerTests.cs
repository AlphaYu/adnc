namespace Adnc.UnitTest.BasicAuthentication;

public class BasicAuthenticationHandlerTests
{
    public BasicAuthenticationHandlerTests()
    {
    }

    [Fact]
    public void PackAndUnPackBase64()
    {
        var token = BasicAuthenticationHandler.PackToBase64("usr");
        var result = BasicAuthenticationHandler.UnpackFromBase64(token, out var userName, out var appId);
        Assert.Equal("usr", userName);
        Assert.NotEmpty(appId);
        Assert.True(result);
    }
}