using Adnc.Infra.Common.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Adnc.Infra.Common.Helper
{
    /// <summary>
    /// HashHelper
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="str">源字符串</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, string str) => GetHashedString(type, str, Encoding.UTF8);

        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="str">源字符串</param>
        /// <param name="isLower">是否是小写</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, string str, bool isLower) => GetHashedString(type, str, Encoding.UTF8, isLower);

        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="str">源字符串</param>
        /// <param name="key">key</param>
        /// <param name="isLower">是否是小写</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, string str, string key, bool isLower = false) => GetHashedString(type, str, key, Encoding.UTF8, isLower);

        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="str">源字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="isLower">是否是小写</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, string str, Encoding encoding, bool isLower = false) => GetHashedString(type, str, null, encoding, isLower);

        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="str">源字符串</param>
        /// <param name="key">key</param>
        /// <param name="encoding">编码类型</param>
        /// <param name="isLower">是否是小写</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, string str, string key, Encoding encoding, bool isLower = false)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : GetHashedString(type, str.GetBytes(encoding), string.IsNullOrEmpty(key) ? null : encoding.GetBytes(key), isLower);
        }

        /// <summary>
        /// 计算字符串Hash值
        /// </summary>
        /// <param name="type">hash类型</param>
        /// <param name="source">source</param>
        /// <returns>hash过的字节数组</returns>
        public static string GetHashedString(HashType type, byte[] source) => GetHashedString(type, source, null);

        /// <summary>
        /// 计算字符串Hash值
        /// </summary>
        /// <param name="type">hash类型</param>
        /// <param name="source">source</param>
        /// <param name="isLower">isLower</param>
        /// <returns>hash过的字节数组</returns>
        public static string GetHashedString(HashType type, byte[] source, bool isLower) => GetHashedString(type, source, null, isLower);

        /// <summary>
        /// 获取哈希之后的字符串
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="source">源</param>
        /// <param name="key">key</param>
        /// <param name="isLower">是否是小写</param>
        /// <returns>哈希算法处理之后的字符串</returns>
        public static string GetHashedString(HashType type, byte[] source, byte[] key, bool isLower = false)
        {
            if (null == source)
            {
                return string.Empty;
            }
            var hashedBytes = GetHashedBytes(type, source, key.IsNotNullOrEmpty() ? key : null);
            var sbText = new StringBuilder();
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
            return sbText.ToString();
        }

        /// <summary>
        /// 计算字符串Hash值
        /// </summary>
        /// <param name="type">hash类型</param>
        /// <param name="str">要hash的字符串</param>
        /// <returns>hash过的字节数组</returns>
        public static byte[] GetHashedBytes(HashType type, string str) => GetHashedBytes(type, str, Encoding.UTF8);

        /// <summary>
        /// 计算字符串Hash值
        /// </summary>
        /// <param name="type">hash类型</param>
        /// <param name="str">要hash的字符串</param>
        /// <param name="encoding">编码类型</param>
        /// <returns>hash过的字节数组</returns>
        public static byte[] GetHashedBytes(HashType type, string str, Encoding encoding)
        {
            if (null == str)
            {
                return null;
            }
            var bytes = encoding.GetBytes(str);
            return GetHashedBytes(type, bytes);
        }

        /// <summary>
        /// 获取Hash后的字节数组
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="bytes">原字节数组</param>
        /// <returns></returns>
        public static byte[] GetHashedBytes(HashType type, byte[] bytes) => GetHashedBytes(type, bytes, null);

        /// <summary>
        /// 获取Hash后的字节数组
        /// </summary>
        /// <param name="type">哈希类型</param>
        /// <param name="key">key</param>
        /// <param name="bytes">原字节数组</param>
        /// <returns></returns>
        public static byte[] GetHashedBytes(HashType type, byte[] bytes, byte[] key)
        {
            if (null == bytes)
            {
                return bytes;
            }

            HashAlgorithm algorithm = null;
            try
            {
                if (key == null)
                {
                    switch (type)
                    {
                        case HashType.MD5:
                            algorithm = MD5.Create();
                            break;

                        case HashType.SHA1:
                            algorithm = SHA1.Create();
                            break;

                        case HashType.SHA256:
                            algorithm = SHA256.Create();
                            break;

                        case HashType.SHA384:
                            algorithm = SHA384.Create();
                            break;

                        case HashType.SHA512:
                            algorithm = SHA512.Create();
                            break;

                        default:
                            algorithm = MD5.Create();
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case HashType.MD5:
                            algorithm = new HMACMD5(key);
                            break;

                        case HashType.SHA1:
                            algorithm = new HMACSHA1(key);
                            break;

                        case HashType.SHA256:
                            algorithm = new HMACSHA256(key);
                            break;

                        case HashType.SHA384:
                            algorithm = new HMACSHA384(key);
                            break;

                        case HashType.SHA512:
                            algorithm = new HMACSHA512(key);
                            break;

                        default:
                            algorithm = new HMACMD5(key);
                            break;
                    }
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
}