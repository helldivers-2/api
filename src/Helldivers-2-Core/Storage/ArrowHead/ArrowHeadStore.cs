using Helldivers.Core.Localization;
using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Storage.ArrowHead;

/// <summary>
/// Dedicated storage for the ArrowHead payloads, as we want to be able to return those byte-by-byte.
/// </summary>
public sealed class ArrowHeadStore
{
    private Memory<byte> _warId;
    private Memory<byte> _warInfo;
    private Memory<byte> _warSummary;
    private CultureDictionary<Memory<byte>> _statuses = null!;
    private CultureDictionary<Memory<byte>> _feeds = null!;
    private CultureDictionary<Memory<byte>> _assignments = null!;
    private Dictionary<long, CultureDictionary<Memory<byte>>> _spaceStations = null!;
    private readonly TaskCompletionSource _syncState = new();

    /// <summary>
    /// Updates the <see cref="ArrowHeadStore" /> with the updated raw values.
    /// </summary>
    public void UpdateRawStore(
        Memory<byte> warId,
        Memory<byte> warInfo,
        Memory<byte> warSummary,
        IEnumerable<KeyValuePair<string, Memory<byte>>> statuses,
        IEnumerable<KeyValuePair<string, Memory<byte>>> feeds,
        IEnumerable<KeyValuePair<string, Memory<byte>>> assignments,
        Dictionary<long, Dictionary<string, Memory<byte>>> spaceStations
    )
    {
        _warId = warId;
        _warInfo = warInfo;
        _warSummary = warSummary;
        _statuses = new(statuses);
        _feeds = new(feeds);
        _assignments = new(assignments);
        _spaceStations =
            spaceStations.ToDictionary(pair => pair.Key, pair => new CultureDictionary<Memory<byte>>(pair.Value));

        _syncState.TrySetResult();
    }

    /// <summary>
    /// returns the raw payload for <see cref="WarId" />.
    /// </summary>
    public async Task<Memory<byte>> GetWarId(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _warId;
    }

    /// <summary>
    /// returns the raw payload for <see cref="WarStatus" />.
    /// </summary>
    public async Task<Memory<byte>> GetWarStatus(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _statuses.Get();
    }

    /// <summary>
    /// returns the raw payload for <see cref="WarInfo" />.
    /// </summary>
    public async Task<Memory<byte>> GetWarInfo(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _warInfo;
    }

    /// <summary>
    /// returns the raw payload for <see cref="WarSummary" />.
    /// </summary>
    public async Task<Memory<byte>> GetWarSummary(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _warSummary;
    }

    /// <summary>
    /// returns the raw payload for <see cref="NewsFeedItem" />s.
    /// </summary>
    public async Task<Memory<byte>> GetNewsFeeds(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _feeds.Get();
    }

    /// <summary>
    /// returns the raw payload for <see cref="Assignment" />s.
    /// </summary>
    public async Task<Memory<byte>> GetAssignments(CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _assignments.Get();
    }

    /// <summary>
    /// returns the raw payload for <see cref="SpaceStation" />s.
    /// </summary>
    public async Task<Memory<byte>?> GetSpaceStation(long id, CancellationToken cancellationToken)
    {
        await _syncState.Task.WaitAsync(cancellationToken);

        return _spaceStations.TryGetValue(id, out var spaceStations) ? spaceStations.Get() : null;
    }
}
