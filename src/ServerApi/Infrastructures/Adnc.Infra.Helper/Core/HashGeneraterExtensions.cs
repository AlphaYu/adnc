namespace Adnc.Infra.Helper;

/// <summary>
/// HashHelper
/// </summary>
public static class HashGeneraterExtensions
{
    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="str">源字符串</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, string str) => GetHashedString(_, type, str, Encoding.UTF8);

    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="str">源字符串</param>
    /// <param name="isLower">是否是小写</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, string str, bool isLower) => GetHashedString(_, type, str, Encoding.UTF8, isLower);

    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="str">源字符串</param>
    /// <param name="key">key</param>
    /// <param name="isLower">是否是小写</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, string str, string key, bool isLower = false) => GetHashedString(_, type, str, key, Encoding.UTF8, isLower);

    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="str">源字符串</param>
    /// <param name="encoding">编码类型</param>
    /// <param name="isLower">是否是小写</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, string str, Encoding encoding, bool isLower = false) => GetHashedString(_, type, str, null, encoding, isLower);

    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="str">源字符串</param>
    /// <param name="key">key</param>
    /// <param name="encoding">编码类型</param>
    /// <param name="isLower">是否是小写</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, string str, string? key, Encoding encoding, bool isLower = false)
    {
        return string.IsNullOrEmpty(str) ? string.Empty : GetHashedString(_, type, str.GetBytes(encoding), string.IsNullOrEmpty(key) ? null : encoding.GetBytes(key), isLower);
    }

    /// <summary>
    /// 计算字符串Hash值
    /// </summary>
    /// <param name="type">hash类型</param>
    /// <param name="source">source</param>
    /// <returns>hash过的字节数组</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, byte[] source) => GetHashedString(_, type, source, null);

    /// <summary>
    /// 计算字符串Hash值
    /// </summary>
    /// <param name="type">hash类型</param>
    /// <param name="source">source</param>
    /// <param name="isLower">isLower</param>
    /// <returns>hash过的字节数组</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, byte[] source, bool isLower) => GetHashedString(_, type, source, null, isLower);

    /// <summary>
    /// 获取哈希之后的字符串
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="source">源</param>
    /// <param name="key">key</param>
    /// <param name="isLower">是否是小写</param>
    /// <returns>哈希算法处理之后的字符串</returns>
    public static string GetHashedString(this IHashGenerater _, HashType type, byte[] source, byte[]? key, bool isLower = false)
    {
        if (null == source)
        {
            return string.Empty;
        }
        var hashedBytes = GetHashedBytes(_, type, source, key);
        var sbText = new StringBuilder();
        if(hashedBytes is not null && hashedBytes.Any())
        {
            if (isLower)
            {
                foreach (var b in hashedBytes)
                {
                    sbText.Append(b.ToString("x2"));
                }
            }
            else
            {
                foreach (var b in hashedBytes)
                {
                    sbText.Append(b.ToString("X2"));
                }
            }
        }
        return sbText.ToString();
    }

    /// <summary>
    /// 计算字符串Hash值
    /// </summary>
    /// <param name="type">hash类型</param>
    /// <param name="str">要hash的字符串</param>
    /// <returns>hash过的字节数组</returns>
    public static byte[]? GetHashedBytes(this IHashGenerater _, HashType type, string str) => GetHashedBytes(_, type, str, Encoding.UTF8);

    /// <summary>
    /// 计算字符串Hash值
    /// </summary>
    /// <param name="type">hash类型</param>
    /// <param name="str">要hash的字符串</param>
    /// <param name="encoding">编码类型</param>
    /// <returns>hash过的字节数组</returns>
    public static byte[]? GetHashedBytes(this IHashGenerater _, HashType type, string str, Encoding encoding)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        var bytes = encoding.GetBytes(str);
        return GetHashedBytes(_, type, bytes);
    }

    /// <summary>
    /// 获取Hash后的字节数组
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="bytes">原字节数组</param>
    /// <returns></returns>
    public static byte[]? GetHashedBytes(this IHashGenerater _, HashType type, byte[] bytes) => GetHashedBytes(_, type, bytes, null);

    /// <summary>
    /// 获取Hash后的字节数组
    /// </summary>
    /// <param name="type">哈希类型</param>
    /// <param name="key">key</param>
    /// <param name="bytes">原字节数组</param>
    /// <returns></returns>
    public static byte[]? GetHashedBytes(this IHashGenerater _, HashType type, byte[] bytes, byte[]? key)
    {
        if (null == bytes)
        {
            return bytes;
        }

        HashAlgorithm? algorithm = null;
        try
        {
            if (key == null)
            {
                algorithm = type switch
                {
                    HashType.MD5 => MD5.Create(),
                    HashType.SHA1 => SHA1.Create(),
                    HashType.SHA256 => SHA256.Create(),
                    HashType.SHA384 => SHA384.Create(),
                    HashType.SHA512 => SHA512.Create(),
                    _ => MD5.Create(),
                };
            }
            else
            {
                algorithm = type switch
                {
                    HashType.MD5 => new HMACMD5(key),
                    HashType.SHA1 => new HMACSHA1(key),
                    HashType.SHA256 => new HMACSHA256(key),
                    HashType.SHA384 => new HMACSHA384(key),
                    HashType.SHA512 => new HMACSHA512(key),
                    _ => new HMACMD5(key),
                };
            }
            return algorithm.ComputeHash(bytes);
        }
        finally
        {
            algorithm?.Dispose();
        }
    }
}

/// <summary>
/// Hash 类型
/// </summary>
public enum HashType
{
    /// <summary>
    /// MD5
    /// </summary>
    MD5 = 0,

    /// <summary>
    /// SHA1
    /// </summary>
    SHA1 = 1,

    /// <summary>
    /// SHA256
    /// </summary>
    SHA256 = 2,

    /// <summary>
    /// SHA384
    /// </summary>
    SHA384 = 3,

    /// <summary>
    /// SHA512
    /// </summary>
    SHA512 = 4
}