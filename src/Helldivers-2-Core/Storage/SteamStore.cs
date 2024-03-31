using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Mapping;
using Helldivers.Models.Domain;
using Helldivers.Models.Steam;

namespace Helldivers.Core.Storage;

public sealed class SteamStore : IStore<SteamNews, string>
{
    private List<SteamNews> _feed = null!;
    private readonly TaskCompletionSource _syncState = new();

    public void UpdateStore(IEnumerable<SteamNewsFeedItem> items)
    {
        _feed = items.Select(SteamNewsMapper.MapToDomain).ToList();

        _syncState.TrySetResult();
    }

    /// <inheritdoc />
    public async Task<List<SteamNews>> AllAsync(CancellationToken cancellationToken = default)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _feed, cancellationToken);

    /// <inheritdoc />
    public async Task<SteamNews?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _feed.FirstOrDefault(
            item => string.Equals(item.Id, key, StringComparison.InvariantCultureIgnoreCase)
        );
    }
}
