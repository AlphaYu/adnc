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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(str, nameof(str));
        var bytes_sha1_in = Encoding.UTF8.GetBytes(str);
        var bytes_sha1_out = SHA1.HashData(bytes_sha1_in);
        var str_sha1_out = BitConverter.ToString(bytes_sha1_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        var bytes_sha256_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_sha256_out = SHA256.HashData(bytes_sha256_in);
        var str_sha256_out = BitConverter.ToString(bytes_sha256_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        var bytes_sha384_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_sha384_out = SHA384.HashData(bytes_sha384_in);
        var str_sha384_out = BitConverter.ToString(bytes_sha384_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        var bytes_sha512_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_sha512_out = SHA512.HashData(bytes_sha512_in);
        var str_sha512_out = BitConverter.ToString(bytes_sha512_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var secrectKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA1(secrectKey);
        hmac.Initialize();

        var bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        var str_hamc_out = BitConverter.ToString(bytes_hamc_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var secrectKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA256(secrectKey);
        hmac.Initialize();

        var bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        var str_hamc_out = BitConverter.ToString(bytes_hamc_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var secrectKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA384(secrectKey);
        hmac.Initialize();

        var bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        var str_hamc_out = BitConverter.ToString(bytes_hamc_out);
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
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var secrectKey = Encoding.UTF8.GetBytes(key);
        using var hmac = new HMACSHA512(secrectKey);
        hmac.Initialize();

        var bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

        var str_hamc_out = BitConverter.ToString(bytes_hamc_out);
        str_hamc_out = str_hamc_out.Replace("-", "");

        return str_hamc_out;
    }
}
