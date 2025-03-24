namespace Adnc.Infra.Unittest.Helper.Tests
{
    public class SHA_Tests
    {
        [Fact(DisplayName = "SHA1 success test")]
        public void SHA1_Success_Test()
        {
            var srcString = "sha encrypt";

            //Ack
            var hashed =  InfraHelper.Encrypt.Sha1(srcString);

            //Assert
            Assert.NotEmpty(hashed);
            Assert.Equal("167e1772beced535bf5d1d8a14d858a68a66eb64", hashed.ToLower());
        }

        [Fact(DisplayName = "SHA1 fail test")]
        public void SHA1_Fail_Test()
        {
            var srcString = string.Empty;
            

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.Sha1(srcString));
        }

        [Fact(DisplayName = "SHA256 success test")]
        public void SHA256_Success_Test()
        {
            var srcString = "sha encrypt";

            //Ack
            var hashed =  InfraHelper.Encrypt.Sha256(srcString);

            //Assert
            Assert.NotEmpty(hashed);
            Assert.Equal("8dfdbf14004e54dfa3b7faa5f59bda6614793bc0e0896086890901c974bb2578", hashed.ToLower());
        }

        [Fact(DisplayName = "SHA256 fail test")]
        public void SHA256_Fail_Test()
        {
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.Sha256(srcString));
        }

        [Fact(DisplayName = "SHA384 success test")]
        public void SHA384_Success_Test()
        {
            var srcString = "sha encrypt";

            //Ack
            var hashed =  InfraHelper.Encrypt.Sha384(srcString);

            //Assert
            Assert.NotEmpty(hashed);
            Assert.Equal("36c125449395902f5e02b6f3546877b662929470aaabf1a1f367bbb846c8a1499c7eebed3e253c05cf944ac4750653e1", hashed.ToLower());
        }

        [Fact(DisplayName = "SHA384 fail test")]
        public void SHA384_Fail_Test()
        {
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.Sha384(srcString));
        }


        [Fact(DisplayName = "SHA512 success test")]
        public void SHA512_Success_Test()
        {
            var srcString = "sha encrypt";

            //Ack
            var hashed =  InfraHelper.Encrypt.Sha512(srcString);

            //Assert
            Assert.NotEmpty(hashed);
            Assert.Equal("08b90382193f2c9ea9d23d55f7416be068ffcbc8fe629f53977475c00e37c566612d7d698a00e813e3715e200a8f78c387c4059aef906ffe4db212f2fa3dea1d", hashed.ToLower());
        }

        [Fact(DisplayName = "SHA512 fail test")]
        public void SHA512_Fail_Test()
        {
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.Sha512(srcString));
        }

    }
}
