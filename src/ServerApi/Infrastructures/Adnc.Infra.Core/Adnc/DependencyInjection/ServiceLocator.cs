namespace Adnc.Infra.Core.DependencyInjection;

/// <summary>
/// Provides access to the root scope IServiceProvider instance
/// </summary>
public static class ServiceLocator
{
    private static IServiceProvider? _provider;

    static ServiceLocator()
    {
    }

    /// <summary>
    /// Gets or sets the current IServiceProvider instance
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if Provider is already set.</exception>
    /// <exception cref="ArgumentNullException">Thrown if value is null when setting Provider.</exception>
    public static IServiceProvider? Provider
    {
        get => _provider;
        set => _provider = _provider is not null ? throw new InvalidOperationException($"{nameof(Provider)} is already set.") : value ?? throw new ArgumentNullException(nameof(value));
    }
}