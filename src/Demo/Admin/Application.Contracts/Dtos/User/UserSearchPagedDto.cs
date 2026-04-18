namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User;

/// <summary>
/// Represents the paging and filtering criteria used to search users.
/// </summary>
public class UserSearchPagedDto : SearchPagedDto
{
    /// <summary>
    /// Gets or sets the user status filter.
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// Gets or sets the department ID filter.
    /// </summary>
    public long? DeptId { get; set; }
}
