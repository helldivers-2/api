using System.Globalization;

namespace Helldivers.Core.Contracts.Collections;

/// <summary>
/// Provides access to the current version of <typeparamref name="T" />.
/// </summary>
/// <remarks>
/// If multiple variants per <see cref="CultureInfo" /> exist, the underlying
/// implementation is responsible for selecting the correct variant
/// based on <see cref="CultureInfo.CurrentCulture" />.
/// </remarks>
public interface IStore<T, in TKey> where T : class
{
    /// <summary>
    /// Fetches all <typeparamref name="T" /> instances available.
    /// </summary>
    Task<List<T>> AllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to fetch a single <typeparamref name="T" /> by <typeparamref name="TKey" /> <paramref name="key" />.
    /// </summary>
    /// <returns>
    /// An instance of <typeparamref name="T" /> if found, or <c>null</c> if no instance associated with <paramref name="key" /> exists.
    /// </returns>
    Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);
}
