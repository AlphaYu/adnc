namespace Adnc.Infra.Helper;

/// <summary>
/// 安全助手
/// </summary>
public static class SecurityExtentions
{
    /// <summary>
    /// get MD5 hashed string
    /// </summary>
    /// <param name="sourceString">原字符串</param>
    /// <param name="isLower">加密后的字符串是否为小写</param>
    /// <returns>加密后字符串</returns>
    public static string MD5(this ISecurity _, string sourceString, bool isLower = false)
    {
        if (string.IsNullOrEmpty(sourceString))
        {
            return "";
        }
        return InfraHelper.Hash.GetHashedString(HashType.MD5, sourceString, isLower);
    }
}