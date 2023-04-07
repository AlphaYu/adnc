using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// SHA1 Encryption
    /// </summary>
    /// <param name="str">The string to be encrypted</param>
    /// <returns></returns>
    public string Sha1(string str)
    {
        Checker.Argument.IsNotEmpty(str, nameof(str));

        using SHA1 sha1 = SHA1.Create();
        byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(str);
        byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
        string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
        str_sha1_out = str_sha1_out.Replace("-", "");
        return str_sha1_out;
    }

    /// <summary>
    /// SHA256 encrypt
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <returns></returns>
    public string Sha256(string srcString)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));

        using SHA256 sha256 = SHA256.Create();
        byte[] bytes_sha256_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_sha256_out = sha256.ComputeHash(bytes_sha256_in);
        string str_sha256_out = BitConverter.ToString(bytes_sha256_out);
        str_sha256_out = str_sha256_out.Replace("-", "");
        return str_sha256_out;
    }

    /// <summary>
    /// SHA384 encrypt
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <returns></returns>
    public string Sha384(string srcString)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));

        using SHA384 sha384 = SHA384.Create();
        byte[] bytes_sha384_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_sha384_out = sha384.ComputeHash(bytes_sha384_in);
        string str_sha384_out = BitConverter.ToString(bytes_sha384_out);
        str_sha384_out = str_sha384_out.Replace("-", "");
        return str_sha384_out;

    }

    /// <summary>
    /// SHA512 encrypt
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <returns></returns>
    public string Sha512(string srcString)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));

        using SHA512 sha512 = SHA512.Create();
        byte[] bytes_sha512_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
        string str_sha512_out = BitConverter.ToString(bytes_sha512_out);
        str_sha512_out = str_sha512_out.Replace("-", "");
        return str_sha512_out;
    }

    /// <summary>
    /// HMAC_SHA1
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <param name="key">encrypte key</param>
    /// <returns></returns>
    public string Sha1HMAC(string srcString, string key)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotEmpty(key, nameof(key));

        byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        using HMACSHA1 hmac = new HMACSHA1(secrectKey);
        hmac.Initialize();

        byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
        str_hamc_out = str_hamc_out.Replace("-", "");

        return str_hamc_out;
    }


    /// <summary>
    /// HMAC_SHA256
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <param name="key">encrypte key</param>
    /// <returns></returns>
    public string Sha256HMAC(string srcString, string key)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotEmpty(key, nameof(key));

        byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        using HMACSHA256 hmac = new HMACSHA256(secrectKey);
        hmac.Initialize();

        byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
        str_hamc_out = str_hamc_out.Replace("-", "");

        return str_hamc_out;
    }

    /// <summary>
    /// HMAC_SHA384
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <param name="key">encrypte key</param>
    /// <returns></returns>
    public string Sha384HMAC(string srcString, string key)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotEmpty(key, nameof(key));

        byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        using HMACSHA384 hmac = new HMACSHA384(secrectKey);
        hmac.Initialize();

        byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);


        string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
        str_hamc_out = str_hamc_out.Replace("-", "");

        return str_hamc_out;
    }

    /// <summary>
    /// HMAC_SHA512
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <param name="key">encrypte key</param>
    /// <returns></returns>
    public string Sha512HMAC(string srcString, string key)
    {
        Checker.Argument.IsNotEmpty(srcString, nameof(srcString));
        Checker.Argument.IsNotEmpty(key, nameof(key));

        byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        using HMACSHA512 hmac = new HMACSHA512(secrectKey);
        hmac.Initialize();

        byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
        str_hamc_out = str_hamc_out.Replace("-", "");

        return str_hamc_out;
    }
}
