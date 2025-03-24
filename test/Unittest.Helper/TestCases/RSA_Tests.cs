using Adnc.Infra.Helper.Internal.Encrypt;

namespace Adnc.Infra.Unittest.Helper.Tests
{
    public class RSA_Tests
    {

        [Theory]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Create_RSAKey_Test(RsaSize size)
        {
            //Act
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);

            //Assert
            Assert.NotNull(rsaKey);
            Assert.NotEmpty(rsaKey.PublicKey);
            Assert.NotEmpty(rsaKey.PrivateKey);
            Assert.NotEmpty(rsaKey.Exponent);
            Assert.NotEmpty(rsaKey.Modulus);
        }

        [Theory]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Rsa_Encrypt_Success_Test(RsaSize size)
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);
            var srcString = "rsa encrypt";

            //Act
            var encrypted =  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString);

            //Assert
            Assert.NotEmpty(encrypted);
        }

        [Theory(DisplayName = "Rsa encrypt with custom RSAEncryptionPadding")]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Rsa_Encrypt_WithPadding_Test(RsaSize size)
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);
            var srcString = "rsa encrypt";

            //Act
            var encrypted =  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString, RSAEncryptionPadding.Pkcs1);

            //Assert
            Assert.NotEmpty(encrypted);
        }


        [Fact(DisplayName = "Rsa encrypt fail with emtpy key")]
        public void Rsa_Encrypt_EmptyKey_Test()
        {
            var key = string.Empty;
            var srcString = "rsa encrypt";

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.RSAEncrypt(key, srcString));
        }

        [Fact(DisplayName = "Rsa encrypt fail with emtpy data")]
        public void Rsa_Encrypt_EmptyData_Test()
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey();
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString));
        }

        [Theory]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Rsa_Decrypt_Success_Test(RsaSize size)
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);
            var srcString = "rsa decrypt";

            //Act
            var encrypted =  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString);
            var decrypted =  InfraHelper.Encrypt.RSADecrypt(rsaKey.PrivateKey, encrypted);

            //Assert
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(decrypted);
            Assert.Equal(srcString, decrypted);
        }


        [Theory(DisplayName = "Rsa decrypt with custom RSAEncryptionPadding")]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Rsa_Decrypt_WithPadding_Test(RsaSize size)
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);
            var srcString = "rsa decrypt";

            //Act
            var padding = RSAEncryptionPadding.Pkcs1;
            var encrypted =  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString, padding);
            var decrypted =  InfraHelper.Encrypt.RSADecrypt(rsaKey.PrivateKey, encrypted, padding);

            //Assert
            Assert.NotEmpty(encrypted);
            Assert.NotEmpty(decrypted);
            Assert.Equal(srcString, decrypted);
        }


        [Fact(DisplayName = "Rsa decrypt fail with emtpy key")]
        public void Rsa_Decrypt_EmptyKey_Test()
        {
            var key = string.Empty;
            var srcString = "rsa decrypt";

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.RSAEncrypt(key, srcString));
        }

        [Fact(DisplayName = "Rsa decrypt fail with emtpy data")]
        public void Rsa_Decrypt_EmptyData_Test()
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey();
            var srcString = string.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>  InfraHelper.Encrypt.RSAEncrypt(rsaKey.PublicKey, srcString));
        }

        [Fact(DisplayName = "Rsa from json string test")]
        public void Rsa_From_JsonString_Test()
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey();

            var publicKey = rsaKey.PublicKey;
            var privateKey = rsaKey.PrivateKey;

            var rsa =  InfraHelper.Encrypt.RSAFromJson(publicKey);

            Assert.NotNull(rsa);

        }

        [Theory(DisplayName = "Rsa encrypt string length limit test")]
        [InlineData(RsaSize.R2048)]
        [InlineData(RsaSize.R3072)]
        [InlineData(RsaSize.R4096)]
        public void Rsa_Encrypt_LengthLimit_Test(RsaSize size)
        {
            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey(size);

            var publicKey = rsaKey.PublicKey;
            var privateKey = rsaKey.PrivateKey;

            //Act
            var rawStr = "eyJNb2R1bHVzIjoidHVSL1V1dFVSV0RSVElDYTFFRDcraUF2MUVnQUl0dC9oNkhHc0x6SG80QXAyVVdqWGtvRkp4T1NuRmdhY3d4cWM0WUg5UDdRaVIxQ1lCK3lvMnJUbkhZbVIrYWs2V3RJRU1YNWtmTTJrWHBNUVY2aFBrd0FxRTFpU1pWRUM2eXlmeTNGZUJTVmNnVlUwMFpJMGozbzhqT3ZMOXhneGhmT1J1eTcwM1RUbXdFPSIsIkV4cG9uZW50IjoiQVFBQiIsIlAiOiI3MVNIYVRnK2JvOXhzRnEzSXlrcHRFUXVHUXZTNDNEUDFoM04xcVlBN1E1VHpoS0IydEc1RWxvamtYTkF4d0VVVStxSnZMWDBxTHdzd09zRkhaL3lydz09IiwiUSI6Inc2R2ltem84a0lUL0xuS2U0Sk5QTUt2YTlVVzBSZUZlVzA5U1ZtVnFVWS9VeHl2eU9kemowd3JzTTZib1ZCU1JnZi9SbUZwRUZ1bUZTVW9yVWkxNVR3PT0iLCJEUCI6Im9yNXpPaXloMzZLeFozKzRhek54aFlDYmJES3JIRGc1VEZ1Ri9rRngvY0V4WWI4YUNFZDJ0ekVPWUxqandxOU1PR2dUYzN5enV3NEN6TWpEK01vc1J3PT0iLCJEUSI6InMvNGhhQVM2K0pVRlhDemxkT2JVTTRuTEdXUWFxempoNGMwbmlvb2d1ZzVGelVMbnlNa3RiRjFlV1YrMTNyWlY4bS8yM2VBZlNaMXRuckw1RE5EK0RRPT0iLCJJbnZlcnNlUSI6IlBPSkRGUk03MmVxd0R3TytldDFpTzIwTWlQcFVEUS93N1hEMHBMLzJWYTE4OEgrRGlaK0NuZDJRdnFYZyt4NFdNZSsrVlVNYXo2bWM3V1g4WnBaWW9RPT0iLCJEIjoiWE1QUEZPYktDcHFON21pNG4zb0tsSmFveTlwdFAwRG9FWXBydGc4NmoyS2RWMWZzQWhJM1JOZTNvRmRMcXhrY0VWWmxTTTNLUmhHeUxnRkY0WDk0cnVIYjBQeC9LZVQxMW1BeDNvQ2NCRVlWelhabXlIUHQzWCs2dlBMZzdmYUhtRmlxK3N0Y2NMTlBNSEdna2lkWTF6NGtiTXZwZnBlOWxhN0VMWUdKM21VPSJ9";

            //RSAEncryptionPaddingMode is Pkcs1
            var padding = RSAEncryptionPadding.Pkcs1;
            var maxLength = ((int)size - 384) / 8 + 37;
            var rawData = rawStr.Substring(0, maxLength);

            var encryptedStr =  InfraHelper.Encrypt.RSAEncrypt(publicKey, rawData, padding);
            var decryptedStr =  InfraHelper.Encrypt.RSADecrypt(privateKey, encryptedStr, padding);

            //RSAEncryptionPaddingMode is Oaep
            padding = RSAEncryptionPadding.OaepSHA1;

            var sha1 = InfraHelper.Encrypt.Sha1("oaep");
            var length = sha1.Length;
            maxLength = (int)size / 8 - 42;   //214 //40 
            rawData = rawStr.Substring(0, maxLength);

            encryptedStr =  InfraHelper.Encrypt.RSAEncrypt(publicKey, rawData, padding);
            decryptedStr =  InfraHelper.Encrypt.RSADecrypt(privateKey, encryptedStr, padding);
            Assert.Equal(decryptedStr, rawData);


            padding = RSAEncryptionPadding.OaepSHA256;

            maxLength = (int)size / 8 - 66;   //190   //64
            rawData = rawStr.Substring(0, maxLength);

            encryptedStr =  InfraHelper.Encrypt.RSAEncrypt(publicKey, rawData, padding);
            decryptedStr =  InfraHelper.Encrypt.RSADecrypt(privateKey, encryptedStr, padding);

            Assert.Equal(decryptedStr, rawData);

            padding = RSAEncryptionPadding.OaepSHA384;
            maxLength = (int)size / 8 - 98;  //158  //96
            rawData = rawStr.Substring(0, maxLength);

            encryptedStr =  InfraHelper.Encrypt.RSAEncrypt(publicKey, rawData, padding);
            decryptedStr =  InfraHelper.Encrypt.RSADecrypt(privateKey, encryptedStr, padding);

            Assert.Equal(decryptedStr, rawData);

            padding = RSAEncryptionPadding.OaepSHA512;
            maxLength = (int)size / 8 - 130; //126  // 128
            rawData = rawStr.Substring(0, maxLength);

            encryptedStr =  InfraHelper.Encrypt.RSAEncrypt(publicKey, rawData, padding);
            decryptedStr =  InfraHelper.Encrypt.RSADecrypt(privateKey, encryptedStr, padding);

            Assert.Equal(decryptedStr, rawData);
        }

        [Fact(DisplayName = "Rsa encrypt out of max length exception test")]
        public void Rsa_Encrypt_OutofMaxLength_Exception_Test()
        {
            //Act
            var rawStr = "eyJNb2R1bHVzIjoidHVSL1V1dFVSV0RSVElDYTFFRDcraUF2MUVnQUl0dC9oNkhHc0x6SG80QXAyVVdqWGtvRkp4T1NuRmdhY3d4cWM0WUg5UDdRaVIxQ1lCK3lvMnJUbkhZbVIrYWs2V3RJRU1YNWtmTTJrWHBNUVY2aFBrd0FxRTFpU1pWRUM2eXlmeTNGZUJTVmNnVlUwMFpJMGozbzhqT3ZMOXhneGhmT1J1eTcwM1RUbXdFPSIsIkV4cG9uZW50IjoiQVFBQiIsIlAiOiI3MVNIYVRnK2JvOXhzRnEzSXlrcHRFUXVHUXZTNDNEUDFoM04xcVlBN1E1VHpoS0IydEc1RWxvamtYTkF4d0VVVStxSnZMWDBxTHdzd09zRkhaL3lydz09IiwiUSI6Inc2R2ltem84a0lUL0xuS2U0Sk5QTUt2YTlVVzBSZUZlVzA5U1ZtVnFVWS9VeHl2eU9kemowd3JzTTZib1ZCU1JnZi9SbUZwRUZ1bUZTVW9yVWkxNVR3PT0iLCJEUCI6Im9yNXpPaXloMzZLeFozKzRhek54aFlDYmJES3JIRGc1VEZ1Ri9rRngvY0V4WWI4YUNFZDJ0ekVPWUxqandxOU1PR2dUYzN5enV3NEN6TWpEK01vc1J3PT0iLCJEUSI6InMvNGhhQVM2K0pVRlhDemxkT2JVTTRuTEdXUWFxempoNGMwbmlvb2d1ZzVGelVMbnlNa3RiRjFlV1YrMTNyWlY4bS8yM2VBZlNaMXRuckw1RE5EK0RRPT0iLCJJbnZlcnNlUSI6IlBPSkRGUk03MmVxd0R3TytldDFpTzIwTWlQcFVEUS93N1hEMHBMLzJWYTE4OEgrRGlaK0NuZDJRdnFYZyt4NFdNZSsrVlVNYXo2bWM3V1g4WnBaWW9RPT0iLCJEIjoiWE1QUEZPYktDcHFON21pNG4zb0tsSmFveTlwdFAwRG9FWXBydGc4NmoyS2RWMWZzQWhJM1JOZTNvRmRMcXhrY0VWWmxTTTNLUmhHeUxnRkY0WDk0cnVIYjBQeC9LZVQxMW1BeDNvQ2NCRVlWelhabXlIUHQzWCs2dlBMZzdmYUhtRmlxK3N0Y2NMTlBNSEdna2lkWTF6NGtiTXZwZnBlOWxhN0VMWUdKM21VPSJ9";

            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey();
            var publicKey = rsaKey.PublicKey;

            //Assert
            Assert.Throws<OutofMaxlengthException>(() =>
            {
                 InfraHelper.Encrypt.RSAEncrypt(publicKey, rawStr);
            });
        }

        [Fact(DisplayName = "Rsa sign and verify test")]
        public void Rsa_SignAndVerify_Test()
        {
            //Act
            var rawStr = "123456";
            var rawStr1 = "123457";

            var rsaKey =  InfraHelper.Encrypt.CreateRsaKey();
            var privateKey = rsaKey.PrivateKey;
            var publicKey = rsaKey.PublicKey;

            var signStr =  InfraHelper.Encrypt.RSASign(rawStr, privateKey);

            var result =  InfraHelper.Encrypt.RSAVerify(rawStr, signStr, publicKey);
            var errorResult =  InfraHelper.Encrypt.RSAVerify(rawStr1, signStr, publicKey);

            //Assert
            Assert.NotEmpty(signStr);
            Assert.True(result);
            Assert.False(errorResult);
        }

        [Theory(DisplayName = "Rsa to pem test")]
        [InlineData(true)]
        [InlineData(false)]
        public void Rsa_To_Pem_Test(bool isPKCS8)
        {
            //Act
            var pemResult =  InfraHelper.Encrypt.RSAToPem(isPKCS8);

            //Assert
            Assert.NotEmpty(pemResult.privatePem);
            Assert.NotEmpty(pemResult.publicPem);
        }


        [Fact(DisplayName = "Rsa from pem test")]
        public void Rsa_From_Pem_Test()
        {
            //Act
            var pemPublicKey = @"
-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxnBvS8cdsnAev2sRDRYWxznm1
QxZzaypfNXLvK7CDGk8TR7K+Pzsa+tpJfoyN/Z4B6xdlpsERo2Cu6AzolvrDLx5w
ZoI0kgdfaBMbUkdOB1m97zFYjKWoPeTskFzWZ3GHcQ3EXT0NJXXFXAskY45vEpbc
5qFgEhcPy3BMqHRibwIDAQAB
-----END PUBLIC KEY-----";

            var rsaFromPublickPem =  InfraHelper.Encrypt.RSAFromPem(pemPublicKey);

            var pemPrivateKey = @"
-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCxnBvS8cdsnAev2sRDRYWxznm1QxZzaypfNXLvK7CDGk8TR7K+
Pzsa+tpJfoyN/Z4B6xdlpsERo2Cu6AzolvrDLx5wZoI0kgdfaBMbUkdOB1m97zFY
jKWoPeTskFzWZ3GHcQ3EXT0NJXXFXAskY45vEpbc5qFgEhcPy3BMqHRibwIDAQAB
AoGAAdwpqm7fxh0S3jOYpJULeQ45gL11dGX7Pp4CWHYzq1vQ14SDtFxYfnLWwGLz
499zvSoSHP1pvjPgz6lxy9Rw8dUxCgvh8VQydMQzaug2XD1tkmtcSWInwFKBAfQ7
rceleyD0aK8JHJiuzM1p+yIJ/ImGK0Zk2U/svqrdJrNR4EkCQQDo3d5iWcjd3OLD
38k1GALEuN17KNpJqLvJcIEJl0pcHtOiNnyy2MR/XUghDpuxwhrhudB/TvX4tuI0
MUeVo5fjAkEAw0D6m9jkwE5uuEYN/l/84rbQ79p2I7r5Sk6zbMyBOvgl6CDlJyxY
434DDm6XW7c55ALrnlratEW5HPiPxuHZBQJANnE4vtGy7nvn4Fd/mRQmAYwe695f
On1iefP9lxpx3huu6uvGN6IKPqS2alQZ/nMdCc0Be+IgC6fmNsGWtNtsdQJAJvB4
ikgxJqD9t8ZQ2CAwgM5Q0OTSlsGdIdKcOeB3DVmbxbV5vdw8RfJFjcVEbkgWRYDH
mKcp4rXc+wgfNFyqOQJATZ1I5ER8AZAn5JMMH9zK+6oFvhLUgKyWO18W+dbcFrBd
AzlTB+HHYEIyTmaDtXWAwgBvJNIHk4BbM1meCH4QnA==
-----END RSA PRIVATE KEY-----";

            var rsaFromPrivatePem =  InfraHelper.Encrypt.RSAFromPem(pemPrivateKey);

            //Assert
            Assert.NotNull(rsaFromPublickPem);
            Assert.NotNull(rsaFromPublickPem);
            Assert.Equal(rsaFromPublickPem.KeySize, rsaFromPrivatePem.KeySize);
            var privateKey =  InfraHelper.Encrypt.CreateRsaKey(rsaFromPrivatePem);
            var publicKey =  InfraHelper.Encrypt.CreateRsaKey(rsaFromPublickPem, false);
            var raw = "123123124";
            var signStr =  InfraHelper.Encrypt.RSASign(raw, privateKey.PrivateKey);
            var result =  InfraHelper.Encrypt.RSAVerify(raw, signStr, publicKey.PublicKey);
            Assert.True(result);
        }


        [Fact(DisplayName = "Rsa encrypt decrypt with pem key test")]
        public void Rsa_Pem_Test()
        {
            //Act
            var rawStr = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQMIGfMA0GCS";

            var pemPublicKey = @"
-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCxnBvS8cdsnAev2sRDRYWxznm1
QxZzaypfNXLvK7CDGk8TR7K+Pzsa+tpJfoyN/Z4B6xdlpsERo2Cu6AzolvrDLx5w
ZoI0kgdfaBMbUkdOB1m97zFYjKWoPeTskFzWZ3GHcQ3EXT0NJXXFXAskY45vEpbc
5qFgEhcPy3BMqHRibwIDAQAB
-----END PUBLIC KEY-----";

            var pemPrivateKey = @"
-----BEGIN RSA PRIVATE KEY-----
MIICWwIBAAKBgQCxnBvS8cdsnAev2sRDRYWxznm1QxZzaypfNXLvK7CDGk8TR7K+
Pzsa+tpJfoyN/Z4B6xdlpsERo2Cu6AzolvrDLx5wZoI0kgdfaBMbUkdOB1m97zFY
jKWoPeTskFzWZ3GHcQ3EXT0NJXXFXAskY45vEpbc5qFgEhcPy3BMqHRibwIDAQAB
AoGAAdwpqm7fxh0S3jOYpJULeQ45gL11dGX7Pp4CWHYzq1vQ14SDtFxYfnLWwGLz
499zvSoSHP1pvjPgz6lxy9Rw8dUxCgvh8VQydMQzaug2XD1tkmtcSWInwFKBAfQ7
rceleyD0aK8JHJiuzM1p+yIJ/ImGK0Zk2U/svqrdJrNR4EkCQQDo3d5iWcjd3OLD
38k1GALEuN17KNpJqLvJcIEJl0pcHtOiNnyy2MR/XUghDpuxwhrhudB/TvX4tuI0
MUeVo5fjAkEAw0D6m9jkwE5uuEYN/l/84rbQ79p2I7r5Sk6zbMyBOvgl6CDlJyxY
434DDm6XW7c55ALrnlratEW5HPiPxuHZBQJANnE4vtGy7nvn4Fd/mRQmAYwe695f
On1iefP9lxpx3huu6uvGN6IKPqS2alQZ/nMdCc0Be+IgC6fmNsGWtNtsdQJAJvB4
ikgxJqD9t8ZQ2CAwgM5Q0OTSlsGdIdKcOeB3DVmbxbV5vdw8RfJFjcVEbkgWRYDH
mKcp4rXc+wgfNFyqOQJATZ1I5ER8AZAn5JMMH9zK+6oFvhLUgKyWO18W+dbcFrBd
AzlTB+HHYEIyTmaDtXWAwgBvJNIHk4BbM1meCH4QnA==
-----END RSA PRIVATE KEY-----";

            var enctypedStr =  InfraHelper.Encrypt.RSAEncryptWithPem(pemPublicKey, rawStr);

            var decryptedStr =  InfraHelper.Encrypt.RSADecryptWithPem(pemPrivateKey, enctypedStr);

            //Assert
            Assert.NotEmpty(enctypedStr);
            Assert.Equal(decryptedStr, rawStr);

        }

        [Fact(DisplayName = "Rsa export pkcs #1 key test")]
        public void Rsa_Pkcs1_Test()
        {
            //Act
            var pkcs1Result =  InfraHelper.Encrypt.RsaToPkcs1();

            //Assert
            Assert.NotEmpty(pkcs1Result.publicPkcs1);
            Assert.NotEmpty(pkcs1Result.privatePkcs1);
        }

        [Fact(DisplayName = "Rsa import pkcs #1 key test")]
        public void Rsa_From_Pkcs1_Test()
        {
            //Act
            var rawString = "test";
            var pkcs1Result =  InfraHelper.Encrypt.RsaToPkcs1();

            var publicKey = pkcs1Result.publicPkcs1;
            var privateKey = pkcs1Result.privatePkcs1;

            var rsa1 =  InfraHelper.Encrypt.RSAFromPublicPkcs(publicKey);
            var rsaKey1 =  InfraHelper.Encrypt.CreateRsaKey(rsa1, false);

            var rsa2 =  InfraHelper.Encrypt.RSAFromPrivatePkcs1(privateKey);
            var rsaKey2 =  InfraHelper.Encrypt.CreateRsaKey(rsa2);

            var encrytpedStr =  InfraHelper.Encrypt.RSAEncrypt(rsaKey1.PublicKey, rawString);
            var decryptedStr =  InfraHelper.Encrypt.RSADecrypt(rsaKey2.PrivateKey, encrytpedStr);

            //Assert
            Assert.NotNull(rsa1);
            Assert.NotNull(rsa2);
            Assert.NotEmpty(encrytpedStr);
            Assert.NotEmpty(decryptedStr);
            Assert.Equal(rawString, decryptedStr);
        }

        [Fact(DisplayName = "Rsa import pkcs #1 string test")]
        public void Rsa_From_Pkcs1Str_Test()
        {
            //Act
            var rawString = "test";

            var publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAyt4/XSnK6rp+FuRfawSsSqZn9kpa2c4c12wEHBa5ygdmL" +
                            "GlkFFOv0XGvvnBhhdyZBa+rrrdWkva+Dl9Zk42DpkDNRpkDpAEvdax0ViqZ4Rq4fK4Gs3uG4fBYIUoV2m55OqqqUzK" +
                            "+hlqBPTyh5jY5rHLZyJYWuWGhlFiM39YALVMvt5DqjaQBW0McrvKkID+y57VDOGS+MolM5yLgu/g0F9ph91zI/47K+" +
                            "421fB3mXd+ecMj82xQiR0MGrP9aZ0MbhAFFzHQrXm709+5zdgioGlXkplyZkjRT6uU55K7qtykhxBuxpbpw5AYPdjLd" +
                            "Mr2X3JjiK8J7zyLI/HimGApO2QIDAQAB";

            var privateKey = "MIIEpAIBAAKCAQEAyt4/XSnK6rp+FuRfawSsSqZn9kpa2c4c12wEHBa5ygdmLGlkFFOv0XGvvnBhhdyZBa+rrrdWkva+" +
                             "Dl9Zk42DpkDNRpkDpAEvdax0ViqZ4Rq4fK4Gs3uG4fBYIUoV2m55OqqqUzK+hlqBPTyh5jY5rHLZyJYWuWGhlFiM39YAL" +
                             "VMvt5DqjaQBW0McrvKkID+y57VDOGS+MolM5yLgu/g0F9ph91zI/47K+421fB3mXd+ecMj82xQiR0MGrP9aZ0MbhAFFz" +
                             "HQrXm709+5zdgioGlXkplyZkjRT6uU55K7qtykhxBuxpbpw5AYPdjLdMr2X3JjiK8J7zyLI/HimGApO2QIDAQABAoIBAC" +
                             "ObdMW2Yy5mA2GjPfg7vr3vjUnWbTHTko5hICuJ9Zw3RkC/Utiag76gVLd5ZDSprpYn6ltlRKXQ9zTwmXljmlrg/ubITrJ" +
                             "HnvvphBXnrlCrbvw7U2PHZ7pahC17onBPeanJcNHfkz9jvVFxRf1xQRg6pG0not9w+npePIPBPRC3PAMrpu3cXC7oqJb4" +
                             "tWGXRX7Gz3ckbkRSvvFQfXr5g/pqtchp7RdFAxroqhGeqn7lsIuXdg21L6IwrUmi4MkW68aUnC53IO3jPncqau10nw2MD" +
                             "dUmLJKdkcXXObIIY5fLQ2kZxuM2QcRf0Xv82OWmx/yUzXbGG3TEgA2HT/RbczECgYEA8ZC9bFUrppe5nxV5PKoXF8LHm5" +
                             "KGYQS+Lci2ZaOoeJMGecSTuDczE+2dWEpRrwG5mrKGGpOnUpqG0l0Ch6wo9QcyPZgaj2Yg5VAcGyaTBddgG1L2PK3wi8b" +
                             "8+IE1zOL5TxoiqSCd5o10LnNTGrqOXy6193XHHPpLc+540lX+7E0CgYEA1v2M64arAY89vtwOA/88O6tAy15Mc19mUoyI" +
                             "PMruQaEe8lgL/1/1HGIt8b2h1wy9AWC+zti5GA/llo/EVJcCzsqalQhpYRrc+a5Bh3jAsFhjrGl0WGDph2QiqbygASNUO" +
                             "kaCv3TzevIkV7lbs16Td10a7oxUeX7qUd8fPZaSQr0CgYEAqev6kv3GWsVXmQPt6DJtVBV7e3+ybwR7EpGhXBWnKEmjwH" +
                             "v6vRZ3I4l9qOF+W/CGsr7pfkBm7sAsHkW2xeDgXpvVR2Z7KGvar/OOEbssqGs7+3x8IWrpTimHQPcC9UCjxnTH9NgwukC" +
                             "+fP46cw7PnzyoW43JpiSads5ExxAe5fECgYEAuxjJLKdz5tWLvK+xGtViy+LLbrDQA4/AcpKOdal9E2xujCUHu+T8YQko" +
                             "RrrrP0V8rthM9aIx4rji6taO27bX4LL9ODmry7AfIsL9kDIMLuudQow7jjY4xXTlnaXj8VmXkWePnaLfyd00t4s+PKlP2" +
                             "I8UQwmo8lr6/OkaPPTusFkCgYAqGXLKncmQsQLVyqNZi/qfdM8cvX8IrpkjyUs3opt+jLVn1JGfTtvWQXisM4KSTpiP/Y" +
                             "7PtDDk3cNm+DlhEz3c3JwjOGuzG0fG8BN5Xak1CR3fp0kuWeoR03EI8WAXR/GYH55TzfVQsbweRXhysr8HhZxezYloiyZ" +
                             "kbQeDzt/P4g==";


            var rsa1 =  InfraHelper.Encrypt.RSAFromPublicPkcs(publicKey);
            var rsaKey1 =  InfraHelper.Encrypt.CreateRsaKey(rsa1, false);

            var rsa2 =  InfraHelper.Encrypt.RSAFromPrivatePkcs1(privateKey);
            var rsaKey2 =  InfraHelper.Encrypt.CreateRsaKey(rsa2);

            var encrytpedStr =  InfraHelper.Encrypt.RSAEncrypt(rsaKey1.PublicKey, rawString);
            var decryptedStr =  InfraHelper.Encrypt.RSADecrypt(rsaKey2.PrivateKey, encrytpedStr);

            //Assert
            Assert.NotNull(rsa1);
            Assert.NotNull(rsa2);
            Assert.NotEmpty(encrytpedStr);
            Assert.NotEmpty(decryptedStr);
            Assert.Equal(rawString, decryptedStr);
        }

        [Fact(DisplayName = "Rsa export pkcs #8 key test")]
        public void Rsa_Pkcs8_Test()
        {
            //Act
            var pkcs1Result =  InfraHelper.Encrypt.RsaToPkcs8();

            //Assert
            Assert.NotEmpty(pkcs1Result.publicPkcs8);
            Assert.NotEmpty(pkcs1Result.privatePkcs8);
        }

        [Fact(DisplayName = "Rsa import pkcs #8 key test")]
        public void Rsa_From_Pkcs8_Test()
        {
            //Act
            var rawStr = "test";
            var pkcs1Result =  InfraHelper.Encrypt.RsaToPkcs8();

            var publicKey = pkcs1Result.publicPkcs8;
            var privateKey = pkcs1Result.privatePkcs8;

            var rsa1 =  InfraHelper.Encrypt.RSAFromPublicPkcs(publicKey);
            var rsaKey1 =  InfraHelper.Encrypt.CreateRsaKey(rsa1, false);

            var rsa2 =  InfraHelper.Encrypt.RSAFromPrivatePkcs8(privateKey);
            var rsaKey2 =  InfraHelper.Encrypt.CreateRsaKey(rsa2);

            var encrytpedStr =  InfraHelper.Encrypt.RSAEncrypt(rsaKey1.PublicKey, rawStr);
            var decryptedStr =  InfraHelper.Encrypt.RSADecrypt(rsaKey2.PrivateKey, encrytpedStr);

            //Assert
            Assert.NotNull(rsa1);
            Assert.NotNull(rsa2);
            Assert.NotEmpty(encrytpedStr);
            Assert.NotEmpty(decryptedStr);
            Assert.Equal(decryptedStr,rawStr );
        }
    }
}
