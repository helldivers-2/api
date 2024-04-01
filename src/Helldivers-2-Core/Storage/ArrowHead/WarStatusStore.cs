using Helldivers.Core.Contracts;
using Helldivers.Core.Localization;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Storage.ArrowHead;

/// <inheritdoc cref="IStore{T}" />
public sealed class WarStatusStore : StoreBase<WarStatus>
{
    private CultureDictionary<WarStatus> _translations = null!;

    /// <inheritdoc cref="IStore{T}.SetStore" />
    public ValueTask SetStore(IEnumerable<KeyValuePair<string, WarStatus>> translations)
    {
        _translations = new(translations);

        return SetStore(_translations.Get()!);
    }

    /// <inheritdoc />
    public override async Task<WarStatus> Get(CancellationToken cancellationToken = default)
    {
        await base.Get(cancellationToken);

        return _translations.Get()!;
    }
}
