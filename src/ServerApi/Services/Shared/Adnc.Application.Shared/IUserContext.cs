using Adnc.Infra.IRepositories;

namespace Adnc.Application.Shared
{
    public interface IUserContext : IOperater
    {
        string RemoteIpAddress { get; set; }

        string Device { get; set; }

        string Email { get; set; }

        long[] RoleIds { get; set; }
    }

    public class UserContext : Operater, IUserContext
    {
        public string RemoteIpAddress { get; set; }

        public string Device { get; set; }

        public string Email { get; set; }

        public long[] RoleIds { get; set; }
    }
}