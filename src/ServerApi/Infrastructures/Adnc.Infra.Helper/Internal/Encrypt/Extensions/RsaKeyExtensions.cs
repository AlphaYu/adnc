using Adnc.Infra.Core.Guard;
using Adnc.Infra.Helper.Internal.Encrypt.Shared;

namespace Adnc.Infra.Helper.Encrypt.Extensions;

/// <summary>
/// RSA Parameter formatting extension
/// </summary>
internal static class RSAKeyExtensions
{
    /// <summary>
    /// RSA import key
    /// </summary>
    /// <param name="rsa">RSA instance<see cref="RSA"/></param>
    /// <param name="jsonString">RSA Key serialization JSON string</param>
    internal static void FromJsonString(this RSA rsa, string jsonString)
    {
        Checker.Argument.IsNotEmpty(jsonString, nameof(jsonString));

        RSAParameters parameters = new RSAParameters();
        try
        {
            var paramsJson =  JsonSerializer.Deserialize<RSAParametersJson>(jsonString);

            parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
            parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
            parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
            parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
            parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
            parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
            parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
            parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
        }
        catch
        {
            throw new Exception("Invalid Json RSA key.");
        }
        rsa.ImportParameters(parameters);
    }

    /// <summary>
    /// Get the RSA Key serialized to Json
    /// </summary>
    /// <param name="rsa">RSA instance<see cref="RSA"/></param>
    /// <param name="includePrivateParameters">Whether to include the private key</param>
    /// <returns></returns>
    internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
    {
        RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        var parasJson = new RSAParametersJson()
        {
            Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
            Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
            P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
            Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
            DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
            DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
            InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
            D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
        };

        return JsonSerializer.Serialize(parasJson);
    }

    /// <summary>
    /// RSA import key
    /// </summary>
    /// <param name="rsa">RSA instance<see cref="RSA"/></param>
    /// <param name="xmlString">RSA Key serialization XML string</param>
    public static void FromLvccXmlString(this RSA rsa, string xmlString)
    {
        RSAParameters parameters = new RSAParameters();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
        {
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus": parameters.Modulus = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "Exponent": parameters.Exponent = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "P": parameters.P = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "Q": parameters.Q = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "DP": parameters.DP = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "DQ": parameters.DQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "InverseQ": parameters.InverseQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                    case "D": parameters.D = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText); break;
                }
            }
        }
        else
        {
            throw new Exception("Invalid XML RSA key.");
        }

        rsa.ImportParameters(parameters);
    }

    /// <summary>
    /// Get the RSA Key as serialized XML
    /// </summary>
    /// <param name="rsa">RSA instance<see cref="RSA"/></param>
    /// <param name="includePrivateParameters">Whether to include the private key</param>
    /// <returns></returns>
    public static string ToLvccXmlString(this RSA rsa, bool includePrivateParameters)
    {
        RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
              parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
              parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
              parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
              parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
              parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
              parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
              parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
              parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
    }
}
