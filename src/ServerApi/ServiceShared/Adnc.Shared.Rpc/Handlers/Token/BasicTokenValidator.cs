namespace Adnc.Shared.Rpc.Handlers.Token;

public static class BasicTokenValidator
{
    public static readonly string InternalCaller = "internalcaller";
    public static readonly Dictionary<string, Partner> Partners;

    static BasicTokenValidator()
    {
        var configuration = ServiceLocator.Provider?.GetService<IConfiguration>();
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));
        var partners = configuration.GetSection(Partner.Name).Get<List<Partner>>();
        Partners = partners.ToDictionary(x => x.UserName);
    }

    public static string PackToBase64(string userName)
    {
        var partnerAccount = IsInternalCaller(userName) ? InternalCaller : userName;
        if (!Partners.ContainsKey(partnerAccount))
            throw new ArgumentNullException(nameof(userName));

        var partner = Partners[partnerAccount];
        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var plainString = $"{partner.AppId}-{partner.SecurityKey}-{currentTotalSeconds}";
        var md5String = InfraHelper.Encrypt.Md5(plainString, true);

        var password = $"{partner.AppId}-{currentTotalSeconds}-{md5String}";
        var basicToken = $"{userName}:{password}";

        var bytes = Encoding.UTF8.GetBytes(basicToken);
        return Convert.ToBase64String(bytes);
    }

    public static UnPackedResult UnPackFromBase64(string base64String)
    {
        var basicToken = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        var credentials = basicToken.Split(':');
        var userName = credentials[0];
        var passwordArrary = credentials[1].Split('-');

        var appId = passwordArrary[0];
        var tokenTotalSeconds = passwordArrary[1].ToLong();
        var tokenMd5String = passwordArrary[2];

        var partnerAccount = IsInternalCaller(userName) ? InternalCaller : userName;
        if (!Partners.ContainsKey(partnerAccount))
            return new UnPackedResult(false, null, null);

        var partner = Partners[partnerAccount];
        if (!partner.AppId.EqualsIgnoreCase(appId))
            return new UnPackedResult(false, null, null);

        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var differenceTotalSeconds = currentTotalSeconds - tokenTotalSeconds;
        if (differenceTotalSeconds > 120 || differenceTotalSeconds < -3)
            return new UnPackedResult(false, null, null);

        var validationMd5String = InfraHelper.Encrypt.Md5($"{partner.AppId}-{partner.SecurityKey}-{tokenTotalSeconds}", true);
        var isSuccessful = validationMd5String.EqualsIgnoreCase(tokenMd5String);
        return new UnPackedResult(isSuccessful, userName, appId);
    }

    public static bool IsInternalCaller(string userName) => userName.StartsWith(InternalCaller);
}

public class UnPackedResult
{
    public UnPackedResult(bool isSuccessful, string? userName, string? appId)
    {
        IsSuccessful = isSuccessful;
        UserName = userName;
        AppId = appId;
    }

    public bool IsSuccessful { get; set; }
    public string? UserName { get; set; }
    public string? AppId { get; set; }
}
