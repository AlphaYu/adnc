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
        [Obsolete($"use {nameof(GetProvider)} instead")]
        get => _provider;
        set => _provider = _provider is not null ? throw new InvalidOperationException($"{nameof(Provider)} is already set.") : value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the current IServiceProvider instance
    /// </summary>
    /// <returns>The current IServiceProvider instance</returns>
    /// <exception cref="InvalidOperationException">Thrown if Provider is not set.</exception>
    public static IServiceProvider GetProvider()
    {
        if (_provider is null)
        {
            throw new InvalidOperationException($"{nameof(Provider)} is not set.");
        }

        return _provider;
    }

    /// <summary>
    /// Checks if the IServiceProvider instance is set
    /// </summary>
    /// <returns>True if the IServiceProvider instance is set; otherwise, false.</returns>
    public static bool HasProvider()
        => _provider is not null;
}
