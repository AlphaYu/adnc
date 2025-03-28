namespace Adnc.Infra.Helper.Internal.Encrypt;

/// <summary>
/// The encrypt string out of max length exception
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="maxLength"></param>
public class OutofMaxlengthException(int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : Exception
{
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

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="maxLength"></param>
    public OutofMaxlengthException(string message, int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : this(maxLength, keySize, rsaEncryptionPadding)
    {
        ErrorMessage = message;
    }
}
