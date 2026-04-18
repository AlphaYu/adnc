namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents validated user information stored for an authenticated session.
/// </summary>
[Serializable]
public record UserValidatedInfoDto : IDto
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserValidatedInfoDto"/> record.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="account">The account name.</param>
    /// <param name="name">The display name.</param>
    /// <param name="roleids">The assigned role IDs.</param>
    /// <param name="roleCodes">The assigned role codes.</param>
    /// <param name="roleNames">The assigned role names.</param>
    /// <param name="status">The account status.</param>
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

    /// <summary>
    /// Gets the user ID.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the account name.
    /// </summary>
    public string Account { get; init; }

    /// <summary>
    /// Gets the display name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the assigned role IDs.
    /// </summary>
    public long[] RoleIds { get; init; }

    /// <summary>
    /// Gets the assigned role codes.
    /// </summary>
    public string[] RoleCodes { get; init; }

    /// <summary>
    /// Gets the assigned role names.
    /// </summary>
    public string[] RoleNames { get; init; }

    /// <summary>
    /// Gets the account status.
    /// </summary>
    public bool Status { get; init; }

    /// <summary>
    /// Gets the validation version used for token validation.
    /// </summary>
    public string ValidationVersion { get; init; }

    /// <summary>
    /// Gets the assigned role IDs as a comma-separated string.
    /// </summary>
    /// <returns>The assigned role IDs as a comma-separated string.</returns>
    public string GetRoleIdsString() => string.Join(',', RoleIds);
}
