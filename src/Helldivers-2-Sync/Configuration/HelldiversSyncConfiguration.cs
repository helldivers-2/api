namespace Helldivers.Sync.Configuration;

/// <summary>
/// Represents configuration for the synchronization system of the Helldivers 2 API.
/// </summary>
public sealed class HelldiversSyncConfiguration
{
    /// <summary>
    /// The interval (in seconds) at which the sync services will fetch information from the API.
    /// </summary>
    public int IntervalSeconds { get; set; } = 10;

    /// <summary>
    /// The default language which the API will use.
    /// </summary>
    public string DefaultLanguage { get; set; } = "en-US";

    /// <summary>
    /// The list of space stations to fetch information for.
    /// </summary>
    public List<long> SpaceStations { get; set; } = new(0);

    /// <summary>
    /// A list of language codes for which translations will be provided.
    /// </summary>
    public List<string> Languages { get; set; } = new(0);

    /// <summary>
    /// Flag to indicate if the application should only run the sync once.
    /// This is used in CI testing to validate sync works.
    /// </summary>
    public bool RunOnce { get; set; } = false;

    /// <summary>
    /// The maximum number of entries ArrowHead returns per newsfeed API call. The sync service pages through
    /// the full history by repeatedly calling the API with an advancing <see cref="NewsFeedFromTimestamp" />
    /// until a page comes back with fewer than this many entries.
    /// </summary>
    public uint NewsFeedMaxEntries { get; set; } = 1024;

    /// <summary>
    /// The timestamp used for the first page of newsfeed entries fetched each sync. Every entry published on
    /// or after this timestamp will eventually be fetched, one <see cref="NewsFeedMaxEntries" />-sized page at a time.
    /// </summary>
    public uint NewsFeedFromTimestamp { get; set; } = 1000;
}
