namespace Adnc.Shared.Remote.Handlers.Token;

public static class BasicTokenValidator
{
    static BasicTokenValidator()
    {
    }

    public static string InternalCaller { get; } = "internal";

    public static string PackToBase64(long userId, BasicOptions authentication)
    {
        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var password = GetEncryptString(userId, authentication, currentTotalSeconds);
        var plainBasicToken = $"{authentication.UserName}:{password}";
        var byteBasicToken = Encoding.UTF8.GetBytes(plainBasicToken);
        return Convert.ToBase64String(byteBasicToken);
    }

    public static UnPackedResult UnPackFromBase64(string base64String, BasicOptions authentication)
    {
        var basicToken = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        var basicUserName = basicToken.Split(":").ElementAt(0);
        var basicPassword = basicToken.Split(":").ElementAt(1);

        try
        {
            var (userId, userName, tokenTotalSeconds) = GetDecryptInfo(basicPassword, authentication.Password);

            if (userName != basicUserName || userId is null)
            {
                return new UnPackedResult(false, null, null);
            }

            var differenceTotalSeconds = DateTime.Now.GetTotalSeconds() - tokenTotalSeconds;
            if (differenceTotalSeconds > 60 || differenceTotalSeconds < -5)
            {
                return new UnPackedResult(false, null, null);
            }
            else
            {
                return new UnPackedResult(true, userName, userId);
            }
        }
        catch
        {
            return new UnPackedResult(false, null, null);
        }
    }

    private static string GetEncryptString(long userId, BasicOptions authentication, double tokenTotalSeconds)
    {
        var userName = authentication.UserName;
        var securityKey = authentication.Password;
        var plainString = $"{userId}-{userName}-{tokenTotalSeconds}";
        var encryptString = InfraHelper.Encrypt.AESEncrypt(plainString, securityKey);
        return encryptString;
    }

    private static (long? userId, string? userName, double tokenTotalSeconds) GetDecryptInfo(string encryptString, string securityKey)
    {
        var plainString = InfraHelper.Encrypt.AESDecrypt(encryptString, securityKey);
        var userId = plainString.Split("-").ElementAt(0).ToLong();
        var userName = plainString.Split("-").ElementAt(1);
        var tokenTotalSeconds = double.Parse(plainString.Split("-").ElementAt(2));
        return (userId, userName, tokenTotalSeconds);
    }
}
