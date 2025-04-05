namespace Adnc.Infra.Helper.Internal.Encrypt;

/// <summary>
/// The encrypt string out of max length exception
/// </summary>
/// <param name="maxLength"></param>
/// <param name="keySize"></param>
/// <param name="rsaEncryptionPadding"></param>
public class OutofMaxlengthException(int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : Exception
{
    /// <summary>
    /// The encrypt string out of max length exception
    /// </summary>
    /// <param name="message"></param>
    /// <param name="maxLength"></param>
    /// <param name="keySize"></param>
    /// <param name="rsaEncryptionPadding"></param>
    public OutofMaxlengthException(string message, int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : this(maxLength, keySize, rsaEncryptionPadding)
    {
        ErrorMessage = message;
    }

    /// <summary>
    /// The max length of ecnrypt data
    /// </summary>
    public int MaxLength { get; private set; } = maxLength;

    /// <summary>
    /// Error message
    /// </summary>

    public string ErrorMessage { get; private set; } = string.Empty;

    /// <summary>
    /// Rsa key size
    /// </summary>
    public int KeySize { get; private set; } = keySize;

    /// <summary>
    /// Rsa padding
    /// </summary>
    public RSAEncryptionPadding RSAEncryptionPadding { get; private set; } = rsaEncryptionPadding;
}
