
namespace Adnc.Shared.WebApi.Authentication
{
    public interface IAuthentication
    {
        Task<Claim[]> ValidateAsync(string securityToken);
    }
}
