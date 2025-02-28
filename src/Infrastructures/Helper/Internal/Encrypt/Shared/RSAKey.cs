namespace Adnc.Infra.Helper.Internal.Encrypt.Shared;

public class RSAKey
{
    /// <summary>
    /// Rsa public key
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;

    /// <summary>
    /// Rsa private key
    /// </summary>
    public string PrivateKey { get; set; } = string.Empty;

    /// <summary>
    /// Rsa public key Exponent
    /// </summary>
    public string Exponent { get; set; } = string.Empty;

    /// <summary>
    /// Rsa public key Modulus
    /// </summary>
    public string Modulus { get; set; } = string.Empty;
}
