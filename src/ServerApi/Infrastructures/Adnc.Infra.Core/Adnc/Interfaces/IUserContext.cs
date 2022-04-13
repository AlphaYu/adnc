namespace Adnc.Infra.Core.Interfaces;

public interface IUserContext : IOperater
{
    string RemoteIpAddress { get; set; }

    string Device { get; set; }

    string Email { get; set; }

    long[] RoleIds { get; set; }
}