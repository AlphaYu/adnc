using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Adnc.Infra.Unittest.Helper.Tests
{
    public class HMACMD5_Tests
    {
        private const string key = "hjajsfp[1nj;ko";

        [Fact(DisplayName = "HMACMD5 success test")]
        public void HMACMD5_Success_Test()
        {
            var srcString = "hmacmd5 value";

            //Act
            var hashed =  InfraHelper.Encrypt.Md5HMAC(srcString, key);

            //Assert
            Assert.NotEmpty(hashed);
            Assert.Equal("625796ca4bee402ce9b0bf048a7eda6d", hashed.ToLower());
        }
    }
}
