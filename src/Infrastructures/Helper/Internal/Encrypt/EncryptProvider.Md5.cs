using Adnc.Infra.Helper.Internal.Encrypt.Shared;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// MD5 hash
    /// </summary>
    /// <param name="srcString">The string to be encrypted.</param>
    /// <param name="isLower"></param>
    /// <param name="length">The length of hash result , default value is <see cref="MD5Length.L32"/>.</param>
    /// <returns></returns>
    public string Md5(string srcString, bool isLower = false, MD5Length length = MD5Length.L32)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        var bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_md5_out = MD5.HashData(bytes_md5_in);

        var str_md5_out = length == MD5Length.L32
    ? BitConverter.ToString(bytes_md5_out)
    : BitConverter.ToString(bytes_md5_out, 4, 8);

        str_md5_out = str_md5_out.Replace("-", "");
        return isLower ? str_md5_out.ToLower() : str_md5_out;
    }

    /// <summary>
    /// Md5HMAC hash
    /// </summary>
    /// <param name="srcString">The string to be encrypted</param>
    /// <param name="isLower"></param>
    /// <param name="key">encrypte key</param>
    /// <returns></returns>
    public string Md5HMAC(string srcString, string key, bool isLower = false)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(srcString, nameof(srcString));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        var secrectKey = Encoding.UTF8.GetBytes(key);
        using var md5 = new HMACMD5(secrectKey);
        var bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
        var bytes_md5_out = md5.ComputeHash(bytes_md5_in);
        var str_md5_out = BitConverter.ToString(bytes_md5_out);
        str_md5_out = str_md5_out.Replace("-", "");
        return isLower ? str_md5_out.ToLower() : str_md5_out;
    }
}
