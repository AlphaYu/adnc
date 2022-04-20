namespace Adnc.Infra.Core.Interfaces;

public interface IUserContext
{
    long Id { get; set; }

    string Account { get; set; }

    string Name { get; set; }

    string RemoteIpAddress { get; set; }

    string Device { get; set; }

    string Email { get; set; }

    long[] RoleIds { get; set; }
}