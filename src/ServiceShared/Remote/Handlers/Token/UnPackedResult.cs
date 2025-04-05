namespace Adnc.Shared.Remote.Handlers.Token;

public class UnPackedResult(bool isSuccessful, string? userName, long? userId)
{
    public bool IsSuccessful { get; set; } = isSuccessful;
    public string? UserName { get; set; } = userName;
    public long? UserId { get; set; } = userId;
}
