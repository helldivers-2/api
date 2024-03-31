using Helldivers.Core.Contracts;
using Helldivers.Core.Contracts.Collections;
using Helldivers.Core.Localization;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Storage;

/// <summary>
/// Implements <see cref="IStore{T}" /> and <see cref="IStore{T,TKey}" /> for all ArrowHead API types.
/// </summary>
public sealed class ArrowHeadStore : IStore<WarInfo>,
    IStore<WarStatus>,
    IStore<WarSummary>,
    IStore<NewsFeedItem, int>,
    IStore<Assignment, int>
{
    private WarInfo _warInfo = null!;
    private CultureDictionary<WarStatus> _status = null!;
    private WarSummary _summary = null!;
    private CultureDictionary<List<NewsFeedItem>> _feed = null!;
    private CultureDictionary<List<Assignment>> _assignments = null!;
    private readonly TaskCompletionSource _syncState = new();

    public void UpdateSnapshot(
        WarInfo warInfo,
        WarSummary summary,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<Assignment>> assignments
    )
    {
        _warInfo = warInfo;
        _status = new(warStatus);
        _summary = summary;
        _feed = new(feed);
        _assignments = new(assignments);

        _syncState.TrySetResult();
    }

    async Task<WarInfo> IStore<WarInfo>.Get(CancellationToken cancellationToken)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _warInfo, cancellationToken);

    async Task<WarStatus> IStore<WarStatus>.Get(CancellationToken cancellationToken)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _status.Get()!, cancellationToken);

    async Task<WarSummary> IStore<WarSummary>.Get(CancellationToken cancellationToken)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _summary, cancellationToken);

    async Task<List<NewsFeedItem>> IStore<NewsFeedItem, int>.AllAsync(CancellationToken cancellationToken)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _feed.Get()!, cancellationToken);

    async Task<NewsFeedItem?> IStore<NewsFeedItem, int>.GetAsync(int key, CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _feed
            .Get()!
            .FirstOrDefault(item => item.Id == key);
    }

    async Task<List<Assignment>> IStore<Assignment, int>.AllAsync(CancellationToken cancellationToken)
        => await _syncState
            .Task
            .WaitAsync(cancellationToken)
            .ContinueWith(_ => _assignments.Get()!, cancellationToken);

    async Task<Assignment?> IStore<Assignment, int>.GetAsync(int key, CancellationToken cancellationToken = default)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _assignments
            .Get()!
            .FirstOrDefault(assignment => assignment.Id32 == key);
    }
}
