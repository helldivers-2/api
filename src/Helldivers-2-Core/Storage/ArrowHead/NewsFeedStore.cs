using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Localization;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Storage.ArrowHead;

/// <inheritdoc cref="IStore{T,TKey}" />
public class NewsFeedStore : StoreBase<NewsFeedItem, int>
{
    private CultureDictionary<List<NewsFeedItem>> _translations = null!;

    /// <inheritdoc cref="IStore{T, TKey}.SetStore" />
    public ValueTask SetStore(IEnumerable<KeyValuePair<string, List<NewsFeedItem>>> translations)
    {
        _translations = new(translations);

        return SetStore(_translations.Get()!);
    }

    /// <inheritdoc />
    public override async Task<List<NewsFeedItem>> AllAsync(CancellationToken cancellationToken = default)
    {
        await base.AllAsync(cancellationToken);

        return _translations.Get()!;
    }

    /// <inheritdoc />
    public override async Task<NewsFeedItem?> GetAsync(int key, CancellationToken cancellationToken = default)
    {
        await base.GetAsync(key, cancellationToken);

        return _translations.Get()?.FirstOrDefault(item => GetAsyncPredicate(item, key));
    }

    /// <inheritdoc />
    protected override bool GetAsyncPredicate(NewsFeedItem item, int key)
        => item.Id == key;
}
