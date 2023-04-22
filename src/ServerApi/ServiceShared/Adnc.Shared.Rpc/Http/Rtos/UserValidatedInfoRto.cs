namespace Adnc.Shared.Rpc.Http.Rtos;

public record UserValidatedInfoRto
{
    public long Id { get; set; } = default;

    public string Account { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    //public string Email { get; set; } = string.Empty;

    public string RoleIds { get; set; } = string.Empty;

    public int Status { get; set; } = default;

    public string ValidationVersion { get; set; } = string.Empty;
}
