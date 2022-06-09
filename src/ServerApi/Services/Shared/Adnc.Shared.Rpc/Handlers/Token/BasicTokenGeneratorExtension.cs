namespace Adnc.Shared.Rpc.Handlers.Token;

public static class BasicTokenGeneratorExtension
{
    public static readonly Dictionary<string, (string partnerId, string securityKey)> Partners = new()
    {
        { "usr", ("10001", "8tf9u") },
        { "maint", ("10002", "66tyr") },
        { "cus", ("10003", "87ygt") },
        { "ord", ("10004", "gh765") },
        { "whse", ("10005", "oi987") }
    };

    public static string PackToBase64(this BasicTokenGenerator _, string userName)
    {
        if (!Partners.ContainsKey(userName))
            return string.Empty;

        var (appId, securityKey) = Partners[userName];
        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var plainString = $"{appId}_{securityKey}_{currentTotalSeconds}";
        var md5String = InfraHelper.Security.MD5(plainString, true);

        var password = $"{appId}_{currentTotalSeconds}_{md5String}";
        var basicToken = $"{userName}:{password}";

        var bytes = Encoding.UTF8.GetBytes(basicToken);
        return Convert.ToBase64String(bytes);
    }

    public static (bool isSuccessful, string userName, string appId) UnPackFromBase64(this BasicTokenGenerator _, string base64String)
    {
        var basicToken = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        var credentials = basicToken.Split(':');
        var packedUserName = credentials[0];
        var packedPassword = credentials[1].Split('_');

        var packedAppId = packedPassword[0];
        var packedTotalSeconds = packedPassword[1].ToLong();
        var packedMd5String = packedPassword[2];

        if (!Partners.ContainsKey(packedUserName))
            return (false, string.Empty, string.Empty);

        var (appId, securityKey) = Partners[packedUserName];
        if (!appId.EqualsIgnoreCase(packedAppId))
            return (false, string.Empty, string.Empty);

        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var differenceTotalSeconds = currentTotalSeconds - packedTotalSeconds;
        if (differenceTotalSeconds > 120 || differenceTotalSeconds < -3)
            return (false, string.Empty, string.Empty);

        var validationMd5String = InfraHelper.Security.MD5($"{appId}_{securityKey}_{packedTotalSeconds}", true);
        var isSuccessful = validationMd5String.EqualsIgnoreCase(packedMd5String);
        return (isSuccessful, packedUserName, packedAppId);
    }
}
