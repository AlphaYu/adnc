namespace Adnc.Demo.Admin.Application.Contracts.Dtos;

[Serializable]
public record UserValidatedInfoDto : IDto
{
    public UserValidatedInfoDto(long id, string account, string name, long[] roleids, string[] roleCodes, string[] roleNames, bool status)
    {
        Id = id;
        Account = account;
        Name = name;
        RoleIds = roleids;
        RoleCodes = roleCodes;
        RoleNames = roleNames;
        Status = status;
        ValidationVersion = Guid.NewGuid().ToString("N");
    }

    public long Id { get; init; }

    public string Account { get; init; }

    public string Name { get; init; }

    public long[] RoleIds { get; init; }

    public string[] RoleCodes { get; init; }

    public string[] RoleNames { get; init; }

    public bool Status { get; init; }

    public string ValidationVersion { get; init; }

    public string GetRoleIdsString() => string.Join(',', RoleIds);
}
