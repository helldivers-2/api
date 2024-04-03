using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Localization;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Storage.ArrowHead;

/// <inheritdoc cref="IStore{T,TKey}" />
public class AssignmentStore : StoreBase<Assignment, int>
{
    private CultureDictionary<List<Assignment>> _translations = null!;

    /// <inheritdoc cref="IStore{T,TKey}.SetStore" />
    public ValueTask SetStore(IEnumerable<KeyValuePair<string, List<Assignment>>> assignments)
    {
        _translations = new(assignments);

        return SetStore(_translations.Get()!);
    }

    /// <inheritdoc />
    public override async Task<List<Assignment>> AllAsync(CancellationToken cancellationToken = default)
    {
        await base.AllAsync(cancellationToken);

        return _translations.Get()!;
    }

    /// <inheritdoc />
    public override async Task<Assignment?> GetAsync(int key, CancellationToken cancellationToken = default)
    {
        await base.GetAsync(key, cancellationToken);

        return _translations.Get()?.FirstOrDefault(item => GetAsyncPredicate(item, key));
    }

    /// <inheritdoc />
    protected override bool GetAsyncPredicate(Assignment assignment, int key)
        => assignment.Id32 == key;
}
