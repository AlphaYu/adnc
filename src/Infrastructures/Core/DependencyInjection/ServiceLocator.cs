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
    [Obsolete($"use {nameof(SetProvider)} && {nameof(GetProvider)} instead")]
    public static IServiceProvider? Provider
    {
        get => _provider;
        set => _provider = _provider is not null ? throw new InvalidOperationException($"{nameof(Provider)} is already set.") : value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Sets the IServiceProvider instance
    /// </summary>
    /// <param name="provider"></param>
    /// <exception cref="InvalidOperationException">Thrown if Provider is already set.</exception>
    /// <exception cref="ArgumentNullException">Thrown if value is null when setting Provider.</exception>
    public static void SetProvider(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        if (_provider is not null)
        {
            throw new InvalidOperationException($"{nameof(_provider)} is already set.");
        }
        _provider = provider;
    }

    /// <summary>
    /// Gets the IServiceProvider instance
    /// </summary>
    /// <returns>The IServiceProvider instance</returns>
    /// <exception cref="InvalidOperationException">Thrown if Provider is not set.</exception>
    public static IServiceProvider GetProvider()
    {
        if (_provider is null)
        {
            throw new InvalidOperationException($"{nameof(_provider)} is not set.");
        }

        return _provider;
    }

    /// <summary>
    /// Checks if the IServiceProvider instance is set
    /// </summary>
    /// <returns>True if the IServiceProvider instance is set; otherwise, false.</returns>
    public static bool HasProvider() => _provider is not null;
}
