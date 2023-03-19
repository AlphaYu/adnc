using Adnc.Infra.Core.Guard;
using Adnc.Infra.Helper.Encrypt.Extensions;
using Adnc.Infra.Helper.Internal.Encrypt.Shared;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// RSA Converter to pem
    /// </summary>
    /// <param name="isPKCS8">true:PKCS8 false:PKCS1</param>
    /// <param name="keySize">Rsa key size ,default is 2048, min value is 2048</param>
    /// <returns></returns>
    public (string publicPem, string privatePem) RSAToPem(bool isPKCS8, int keySize = 2048)
    {
        if (keySize < 2048)
        {
            throw new ArgumentException($" Key size min value is 2048!");
        }

        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = keySize;

            var publicPem = RsaProvider.ToPem(rsa, false, isPKCS8);
            var privatePem = RsaProvider.ToPem(rsa, true, isPKCS8);

            return (publicPem, privatePem);
        }
    }

    /// <summary>
    /// RSA From pem
    /// </summary>
    /// <param name="pem"></param>
    /// <returns></returns>
    public RSA RSAFromPem(string pem)
    {
        Checker.Argument.IsNotEmpty(pem, nameof(pem));
        return RsaProvider.FromPem(pem);
    }

    /// <summary>
    /// Export Rsa PKCS1 key
    /// </summary>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public (string publicPkcs1, string privatePkcs1) RsaToPkcs1(int keySize = 2048)
    {
        if (keySize < 2048)
        {
            throw new ArgumentException($" Key size min value is 2048!");
        }

        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = keySize;
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

            return (publicKey, privateKey);
        }
    }

    /// <summary>
    /// Export Rsa PKCS8 key
    /// </summary>
    /// <param name="keySize"></param>
    /// <returns></returns>
    public (string publicPkcs8, string privatePkcs8) RsaToPkcs8(int keySize = 2048)
    {
        if (keySize < 2048)
        {
            throw new ArgumentException($" Key size min value is 2048!");
        }

        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = keySize;

            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());

            return (publicKey, privateKey);
        }
    }

    /// <summary>
    /// RSA From pkcs public key
    /// </summary>
    /// <param name="pkcsKey"></param>
    /// <returns></returns>
    public RSA RSAFromPublicPkcs(string pkcsKey)
    {
        return RSAFromPkcs(pkcsKey, false);
    }

    /// <summary>
    ///  RSA From pkcs #1 private key
    /// </summary>
    /// <param name="pkcsKey"></param>
    /// <returns></returns>
    public RSA RSAFromPrivatePkcs1(string pkcsKey)
    {
        return RSAFromPkcs(pkcsKey, true);
    }

    /// <summary>
    ///  RSA From pkcs #8 private key
    /// </summary>
    /// <param name="pkcsKey"></param>
    /// <returns></returns>
    public RSA RSAFromPrivatePkcs8(string pkcsKey)
    {
        return RSAFromPkcs(pkcsKey, true, true);
    }

    /// <summary>
    /// RSA From pkcs#1 or pkcs#8
    /// </summary>
    /// <param name="pkcsKey">Pkcs #1 or Pkcs #8</param>
    /// <param name="isPrivateKey">true:privateKey,false:publicKey</param>
    /// <param name="isPKCS8">true:PKCS8 false:PKCS1</param>
    /// <returns></returns>
    public RSA RSAFromPkcs(string pkcsKey, bool isPrivateKey, bool isPKCS8 = false)
    {
        Checker.Argument.IsNotEmpty(pkcsKey, nameof(pkcsKey));

        RSA rsa = RSA.Create();

        var keySource = Convert.FromBase64String(pkcsKey);

        if (!isPrivateKey)
        {
            try
            {
                rsa.ImportRSAPublicKey(keySource, out _);
            }
            catch
            {
                rsa.ImportSubjectPublicKeyInfo(keySource, out _);
            }
        }
        else
        {
            if (isPKCS8)
            {
                rsa.ImportPkcs8PrivateKey(keySource, out _);
            }
            else
            {
                rsa.ImportRSAPrivateKey(keySource, out _);
            }
        }

        return rsa;

    }

    /// <summary>
    /// RSA Sign
    /// </summary>
    /// <param name="conent">raw cotent </param>
    /// <param name="privateKey">private key</param>
    /// <returns></returns>
    public string RSASign(string conent, string privateKey)
    {
        return RSASign(conent, privateKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1, Encoding.UTF8);
    }

    /// <summary>
    /// RSA Sign
    /// </summary>
    /// <param name="content">raw content </param>
    /// <param name="privateKey">private key</param>
    /// <param name="hashAlgorithmName">hashAlgorithm name</param>
    /// <param name="rSASignaturePadding">ras siginature padding</param>
    /// <param name="encoding">text encoding</param>
    /// <returns></returns>
    public string RSASign(string content, string privateKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
    {
        Checker.Argument.IsNotEmpty(content, nameof(content));
        Checker.Argument.IsNotEmpty(privateKey, nameof(privateKey));
        Checker.Argument.IsNotNull(rSASignaturePadding, nameof(rSASignaturePadding));

        byte[] dataBytes = encoding.GetBytes(content);

        using (RSA rsa = RSA.Create())
        {
            rsa.FromJsonString(privateKey);
            var signBytes = rsa.SignData(dataBytes, hashAlgorithmName, rSASignaturePadding);

            return Convert.ToBase64String(signBytes);
        }
    }

    /// <summary>
    /// RSA Verify
    /// </summary>
    /// <param name="content">raw content</param>
    /// <param name="signStr">sign str</param>
    /// <param name="publickKey">public key</param>
    /// <returns></returns>
    public bool RSAVerify(string content, string signStr, string publickKey)
    {
        return RSAVerify(content, signStr, publickKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1, Encoding.UTF8);
    }

    /// <summary>
    /// RSA Verify
    /// </summary>
    /// <param name="content">raw content</param>
    /// <param name="signStr">sign str</param>
    /// <param name="publickKey">public key</param>
    /// <param name="hashAlgorithmName">hashAlgorithm name</param>
    /// <param name="rSASignaturePadding">ras siginature padding</param>
    /// <param name="encoding">text encoding</param>
    /// <returns></returns>
    public bool RSAVerify(string content, string signStr, string publickKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
    {
        Checker.Argument.IsNotEmpty(content, nameof(content));
        Checker.Argument.IsNotEmpty(signStr, nameof(signStr));

        byte[] dataBytes = encoding.GetBytes(content);
        byte[] signBytes = Convert.FromBase64String(signStr);

        using (RSA rsa = RSA.Create())
        {
            rsa.FromJsonString(publickKey);
            return rsa.VerifyData(dataBytes, signBytes, hashAlgorithmName, rSASignaturePadding);
        }
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="srcString">src string</param>
    /// <returns>encrypted string</returns>
    public string RSAEncrypt(string publicKey, string srcString)
    {
        string encryptStr = RSAEncrypt(publicKey, srcString, RSAEncryptionPadding.OaepSHA512);
        return encryptStr;
    }

    /// <summary>
    /// RSA encrypt with pem key
    /// </summary>
    /// <param name="publicKey">pem public key</param>
    /// <param name="srcString">src string</param>
    /// <returns></returns>
    public string RSAEncryptWithPem(string publicKey, string srcString)
    {
        string encryptStr = RSAEncrypt(publicKey, srcString, RSAEncryptionPadding.Pkcs1, true);
        return encryptStr;
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="srcString">src string</param>
    /// <param name="padding">rsa encryptPadding <see cref="RSAEncryptionPadding"/> RSAEncryptionPadding.Pkcs1 for linux/mac openssl </param>
    /// <param name="isPemKey">set key is pem format,default is false</param>
    /// <returns>encrypted string</returns>
    public string RSAEncrypt(string publicKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
    {
        Checker.Argument.IsNotEmpty(publicKey, nameof(publicKey));
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotNull(padding, nameof(padding));

        RSA rsa;
        if (isPemKey)
        {
            rsa = RsaProvider.FromPem(publicKey);
        }
        else
        {
            rsa = RSA.Create();
            rsa.FromJsonString(publicKey);
        }

        using (rsa)
        {
            var maxLength = GetMaxRsaEncryptLength(rsa, padding);
            var rawBytes = Encoding.UTF8.GetBytes(srcString);

            if (rawBytes.Length > maxLength)
            {
                throw new OutofMaxlengthException($"'{srcString}' is out of max encrypt length {maxLength}", maxLength, rsa.KeySize, padding);
            }

            byte[] encryptBytes = rsa.Encrypt(rawBytes, padding);
            return encryptBytes.ToHexString();
        }
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="data">data byte[]</param>
    /// <returns>encrypted byte[]</returns>
    public byte[] RSAEncrypt(string publicKey, byte[] data)
    {
        byte[] encryptBytes = RSAEncrypt(publicKey, data, RSAEncryptionPadding.OaepSHA512);
        return encryptBytes;
    }

    /// <summary>
    /// RSA encrypt with pem key
    /// </summary>
    /// <param name="publicKey">pem public key</param>
    /// <param name="data">data byte[]</param>
    /// <returns></returns>
    public byte[] RSAEncryptWithPem(string publicKey, byte[] data)
    {
        byte[] encryptBytes = RSAEncrypt(publicKey, data, RSAEncryptionPadding.Pkcs1, true);
        return encryptBytes;
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="data">data byte[]</param>
    /// <param name="padding">rsa encryptPadding <see cref="RSAEncryptionPadding"/> RSAEncryptionPadding.Pkcs1 for linux/mac openssl </param>
    /// <param name="isPemKey">set key is pem format,default is false</param>
    /// <returns>encrypted byte[]</returns>
    public byte[] RSAEncrypt(string publicKey, byte[] data, RSAEncryptionPadding padding, bool isPemKey = false)
    {
        Checker.Argument.IsNotEmpty(publicKey, nameof(publicKey));
        Checker.Argument.IsNotNull(data, nameof(data));
        Checker.Argument.IsNotNull(padding, nameof(padding));

        RSA rsa;
        if (isPemKey)
        {
            rsa = RsaProvider.FromPem(publicKey);
        }
        else
        {
            rsa = RSA.Create();
            rsa.FromJsonString(publicKey);
        }

        using (rsa)
        {
            var maxLength = GetMaxRsaEncryptLength(rsa, padding);
            var rawBytes = data;

            if (rawBytes.Length > maxLength)
            {
                throw new OutofMaxlengthException($"data is out of max encrypt length {maxLength}", maxLength, rsa.KeySize, padding);
            }

            byte[] encryptBytes = rsa.Encrypt(rawBytes, padding);
            return encryptBytes;
        }
    }

    /// <summary>
    /// RSA decrypt
    /// </summary>
    /// <param name="privateKey">private key</param>
    /// <param name="srcString">encrypted string</param>
    /// <returns>Decrypted string</returns>
    public string RSADecrypt(string privateKey, string srcString)
    {
        string decryptStr = RSADecrypt(privateKey, srcString, RSAEncryptionPadding.OaepSHA512);
        return decryptStr;
    }

    /// <summary>
    /// RSA decrypt with pem key
    /// </summary>
    /// <param name="privateKey">pem private key</param>
    /// <param name="scrString">src string</param>
    /// <returns></returns>
    public string RSADecryptWithPem(string privateKey, string srcString)
    {
        string decryptStr = RSADecrypt(privateKey, srcString, RSAEncryptionPadding.Pkcs1, true);
        return decryptStr;
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="srcString">src string</param>
    /// <param name="padding">rsa encryptPadding <see cref="RSAEncryptionPadding"/> RSAEncryptionPadding.Pkcs1 for linux/mac openssl </param>
    /// <param name="isPemKey">set key is pem format,default is false</param>
    /// <returns>encrypted string</returns>
    public string RSADecrypt(string privateKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
    {
        Checker.Argument.IsNotEmpty(privateKey, nameof(privateKey));
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotNull(padding, nameof(padding));

        RSA rsa;
        if (isPemKey)
        {
            rsa = RsaProvider.FromPem(privateKey);
        }
        else
        {
            rsa = RSA.Create();
            rsa.FromJsonString(privateKey);
        }

        using (rsa)
        {
            byte[] srcBytes = srcString.ToBytes();
            byte[] decryptBytes = rsa.Decrypt(srcBytes, padding);
            return Encoding.UTF8.GetString(decryptBytes);
        }
    }

    /// <summary>
    /// RSA decrypt
    /// </summary>
    /// <param name="privateKey">private key</param>
    /// <param name="data">encrypted byte[]</param>
    /// <returns>Decrypted string</returns>
    public byte[] RSADecrypt(string privateKey, byte[] data)
    {
        byte[] decryptBytes = RSADecrypt(privateKey, data, RSAEncryptionPadding.OaepSHA512);
        return decryptBytes;
    }

    /// <summary>
    /// RSA decrypt with pem key
    /// </summary>
    /// <param name="privateKey">pem private key</param>
    /// <param name="data">encrypted byte[]</param>
    /// <returns></returns>
    public byte[] RSADecryptWithPem(string privateKey, byte[] data)
    {
        byte[] decryptBytes = RSADecrypt(privateKey, data, RSAEncryptionPadding.Pkcs1, true);
        return decryptBytes;
    }

    /// <summary>
    /// RSA encrypt
    /// </summary>
    /// <param name="publicKey">public key</param>
    /// <param name="data">src string</param>
    /// <param name="padding">rsa encryptPadding <see cref="RSAEncryptionPadding"/> RSAEncryptionPadding.Pkcs1 for linux/mac openssl </param>
    /// <param name="isPemKey">set key is pem format,default is false</param>
    /// <returns>encrypted string</returns>
    public byte[] RSADecrypt(string privateKey, byte[] data, RSAEncryptionPadding padding, bool isPemKey = false)
    {
        Checker.Argument.IsNotEmpty(privateKey, nameof(privateKey));
        Checker.Argument.IsNotNull(data, nameof(data));
        Checker.Argument.IsNotNull(padding, nameof(padding));

        RSA rsa;
        if (isPemKey)
        {
            rsa = RsaProvider.FromPem(privateKey);
        }
        else
        {
            rsa = RSA.Create();
            rsa.FromJsonString(privateKey);
        }

        using (rsa)
        {
            byte[] srcBytes = data;
            byte[] decryptBytes = rsa.Decrypt(srcBytes, padding);
            return decryptBytes;
        }
    }

    /// <summary>
    /// RSA from json string
    /// </summary>
    /// <param name="rsaKey">rsa json key</param>
    /// <returns></returns>
    public RSA RSAFromJson(string rsaKey)
    {
        Checker.Argument.IsNotEmpty(rsaKey, nameof(rsaKey));
        RSA rsa = RSA.Create();

        rsa.FromJsonString(rsaKey);
        return rsa;
    }

    /// <summary>
    /// Create an RSA key
    /// </summary>
    /// <param name="keySizeInBits">the default size is 2048</param>
    /// <returns></returns>
    public RSAKey CreateRsaKey(RsaSize rsaSize = RsaSize.R2048)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.KeySize = (int)rsaSize;

            string publicKey = rsa.ToJsonString(false);
            string privateKey = rsa.ToJsonString(true);
            var exponent = rsa.ExportParameters(false).Exponent ?? throw new NullReferenceException("rsa.ExportParameters(false).Exponent is null");
            var modulus = rsa.ExportParameters(false).Modulus ?? throw new NullReferenceException("rsa.ExportParameters(false).Modulus is null");

            return new RSAKey()
            {
                PublicKey = publicKey,
                PrivateKey = privateKey,
                Exponent = exponent.ToHexString(),
                Modulus = modulus.ToHexString()
            };
        }
    }

    /// <summary>
    /// Create an RSA key
    /// </summary>
    /// <param name="rsa">rsa</param>
    /// <param name="includePrivate"></param>
    /// <returns></returns>
    public RSAKey CreateRsaKey(RSA rsa, bool includePrivate = true)
    {
        Checker.Argument.IsNotNull(rsa, nameof(rsa));

        string publicKey = rsa.ToJsonString(false);
        var exponent = rsa.ExportParameters(false).Exponent ?? throw new NullReferenceException("rsa.ExportParameters(false).Exponent is null");
        var modulus = rsa.ExportParameters(false).Modulus ?? throw new NullReferenceException("rsa.ExportParameters(false).Modulus is null");

        var rsaKey = new RSAKey()
        {
            PublicKey = publicKey,

            Exponent = exponent.ToHexString(),
            Modulus = modulus.ToHexString()
        };

        if (includePrivate)
        {
            string privateKey = rsa.ToJsonString(true);
            rsaKey.PrivateKey = privateKey;
        }
        return rsaKey;
    }

    /// <summary>
    /// Get rsa encrypt max length
    /// </summary>
    /// <param name="rsa">Rsa instance </param>
    /// <param name="padding"><see cref="RSAEncryptionPadding"/></param>
    /// <returns></returns>
    private int GetMaxRsaEncryptLength(RSA rsa, RSAEncryptionPadding padding)
    {
        var offset = 0;
        if (padding.Mode == RSAEncryptionPaddingMode.Pkcs1)
        {
            offset = 11;
        }
        else
        {
            if (padding.Equals(RSAEncryptionPadding.OaepSHA1))
            {
                offset = 42;
            }

            if (padding.Equals(RSAEncryptionPadding.OaepSHA256))
            {
                offset = 66;
            }

            if (padding.Equals(RSAEncryptionPadding.OaepSHA384))
            {
                offset = 98;
            }

            if (padding.Equals(RSAEncryptionPadding.OaepSHA512))
            {
                offset = 130;
            }
        }
        var keySize = rsa.KeySize;
        var maxLength = keySize / 8 - offset;
        return maxLength;
    }
}
