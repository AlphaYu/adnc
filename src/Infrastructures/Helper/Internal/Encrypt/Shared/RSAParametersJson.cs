namespace Adnc.Infra.Helper.Internal.Encrypt.Shared;

internal class RSAParametersJson
{
    //Public key Modulus
    public string Modulus { get; set; } = string.Empty;
    //Public key Exponent
    public string Exponent { get; set; } = string.Empty;

    public string P { get; set; } = string.Empty;

    public string Q { get; set; } = string.Empty;

    public string DP { get; set; } = string.Empty;

    public string DQ { get; set; } = string.Empty;

    public string InverseQ { get; set; } = string.Empty;

    public string D { get; set; } = string.Empty;
}
