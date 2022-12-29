using Adnc.Infra.Core.Guard;

namespace Adnc.Infra.Helper.Internal.Encrypt;

public partial class EncryptProivder
{
    /// <summary>
    /// Base64 encrypt
    /// </summary>
    /// <param name="input">input value</param>
    /// <returns></returns>
    public string Base64Encrypt(string input)
    {
        return Base64Encrypt(input, Encoding.UTF8);
    }

    /// <summary>
    /// Base64 encrypt
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="encoding">text encoding</param>
    /// <returns></returns>
    public string Base64Encrypt(string input, Encoding encoding)
    {
        Checker.Argument.IsNotEmpty(input, nameof(input));
        return Convert.ToBase64String(encoding.GetBytes(input));
    }

    /// <summary>
    /// Base64 decrypt
    /// </summary>
    /// <param name="input">input value</param>
    /// <returns></returns>
    public string Base64Decrypt(string input)
    {
        return Base64Decrypt(input, Encoding.UTF8);
    }

    /// <summary>
    /// Base64 decrypt
    /// </summary>
    /// <param name="input">input value</param>
    /// <param name="encoding">text encoding</param>
    /// <returns></returns>
    public string Base64Decrypt(string input, Encoding encoding)
    {
        Checker.Argument.IsNotEmpty(input, nameof(input));
        return encoding.GetString(Convert.FromBase64String(input));
    }
}
