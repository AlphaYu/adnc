namespace Adnc.Shared.Remote.Http.Messages;

public record UserValidatedInfoResponse
{
    public bool Status { get; set; }

    public string ValidationVersion { get; set; } = string.Empty;
}
