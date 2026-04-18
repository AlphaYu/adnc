namespace Adnc.Shared.Remote.Handlers.Token;

public interface ITokenGenerator
{
    public static string Scheme { get; } = string.Empty;

    public string GeneratorName { get; }

    /// <summary>
    /// Creates or retrieves a token
    /// </summary>
    /// <returns></returns>
    public string Create();
}
