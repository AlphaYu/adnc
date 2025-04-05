namespace Adnc.Shared.WebApi.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class AdncAuthorizeAttribute : AuthorizeAttribute
{
    public const string JwtWithBasicSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{Authentication.Basic.BasicDefaults.AuthenticationScheme}";

    public AdncAuthorizeAttribute(string code, string schemes = JwtBearerDefaults.AuthenticationScheme)
        : this([code], schemes)
    {
    }

    public AdncAuthorizeAttribute(string[] codes, string schemes = JwtBearerDefaults.AuthenticationScheme)
    {
        Codes = codes;
        Policy = AuthorizePolicy.Default;
        AuthenticationSchemes = schemes ?? throw new ArgumentNullException(nameof(schemes)); ;
    }

    public string[] Codes { get; set; }
}
