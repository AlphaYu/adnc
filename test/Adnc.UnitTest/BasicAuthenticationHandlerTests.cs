﻿using Adnc.Shared.Rpc.Http.Handlers.Token;

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
        var result= BasicTokenValidator.UnPackFromBase64(token);
        Assert.Equal(BasicTokenValidator.InternalCaller, result.UserName);
        Assert.NotEmpty(result.AppId);
        Assert.True(result.IsSuccessful);
    }
}