using Adnc.Infra.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Shared.Rpc.Handlers.Token;

public static class BasicTokenValidator
{
    public static readonly string InternalCaller = "internalcaller";
    public static readonly Dictionary<string, Partner> Partners;

    static BasicTokenValidator()
    {
        var configuration = ServiceLocator.Provider.GetService<IConfiguration>();
        var partners = configuration.GetRpcPartnersSection().Get<List<Partner>>();
        Partners = partners.ToDictionary(x => x.UserName);
    }

    public static string PackToBase64(string userName)
    {
        if (!Partners.ContainsKey(userName))
            return string.Empty;

        var partner = Partners[userName];
        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var plainString = $"{partner.AppId}_{partner.SecurityKey}_{currentTotalSeconds}";
        var md5String = InfraHelper.Security.MD5(plainString, true);

        var password = $"{partner.AppId}_{currentTotalSeconds}_{md5String}";
        var basicToken = $"{userName}:{password}";

        var bytes = Encoding.UTF8.GetBytes(basicToken);
        return Convert.ToBase64String(bytes);
    }

    public static (bool isSuccessful, string userName, string appId) UnPackFromBase64(string base64String)
    {
        var basicToken = Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        var credentials = basicToken.Split(':');
        var userName = credentials[0];
        var passwordArrary = credentials[1].Split('_');

        var appId = passwordArrary[0];
        var tokenTotalSeconds = passwordArrary[1].ToLong();
        var tokenMd5String = passwordArrary[2];

        if (!Partners.ContainsKey(userName))
            return (false, string.Empty, string.Empty);

        var partner = Partners[userName];
        if (!partner.AppId.EqualsIgnoreCase(appId))
            return (false, string.Empty, string.Empty);

        var currentTotalSeconds = DateTime.Now.GetTotalSeconds();
        var differenceTotalSeconds = currentTotalSeconds - tokenTotalSeconds;
        if (differenceTotalSeconds > 120 || differenceTotalSeconds < -3)
            return (false, string.Empty, string.Empty);

        var validationMd5String = InfraHelper.Security.MD5($"{partner.AppId}_{partner.SecurityKey}_{tokenTotalSeconds}", true);
        var isSuccessful = validationMd5String.EqualsIgnoreCase(tokenMd5String);
        return (isSuccessful, userName, appId);
    }
}

public class Partner
{
    public string UserName { get; set; } = string.Empty;
    public string AppId { get; set; } = string.Empty;
    public string SecurityKey { get; set; } = string.Empty;
}
