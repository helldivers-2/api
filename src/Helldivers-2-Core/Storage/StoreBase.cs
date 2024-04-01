using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;

namespace Helldivers.Core.Storage;

/// <summary>
/// Base class for <see cref="IStore{T}" />
/// </summary>
public abstract class StoreBase<T> : IStore<T> where T : class
{
    protected T _state = null!;
    protected readonly TaskCompletionSource _syncState = new();

    /// <summary>
    /// Updates the state of the store with the new value.
    /// </summary>
    public virtual ValueTask SetStore(T value)
    {
        _state = value;
        _syncState.SetResult();

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public virtual async Task<T> Get(CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _state;
    }
}

/// <summary>
/// Base class for <see cref="IStore{T, TKey}" />
/// </summary>
public abstract class StoreBase<T, TKey> : IStore<T, TKey> where T : class
{
    protected List<T> _state = null!;
    protected readonly TaskCompletionSource _syncState = new();

    /// <summary>
    /// Updates the state of the store with the new value.
    /// </summary>
    public virtual ValueTask SetStore(List<T> value)
    {
        _state = value;
        _syncState.SetResult();

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public virtual async Task<List<T>> AllAsync(CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _state;
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _state.FirstOrDefault(value => GetAsyncPredicate(value, key));
    }

    /// <summary>
    /// Handles the filter for <see cref="GetAsync" />
    /// </summary>
    protected abstract bool GetAsyncPredicate(T item, TKey key);
}
