using System.Globalization;

namespace Helldivers.Core.Contracts;

/// <summary>
/// Provides access to the current version of <typeparamref name="T" />.
/// </summary>
/// <remarks>
/// If multiple variants per <see cref="CultureInfo" /> exist, the underlying
/// implementation is responsible for selecting the correct variant
/// based on <see cref="CultureInfo.CurrentCulture" />.
/// </remarks>
public interface IStore<T> where T : class
{
    /// <summary>
    /// Updates the state of the store with the given <paramref name="value" />.
    /// </summary>
    ValueTask SetStore(T value);

    /// <summary>
    /// Gets the current snapshot of <typeparamref name="T" />.
    /// </summary>
    Task<T> Get(CancellationToken cancellationToken = default);
}
