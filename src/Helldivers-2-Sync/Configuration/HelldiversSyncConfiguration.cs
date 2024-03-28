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
    /// A list of language codes for which translations will be provided.
    /// </summary>
    public List<string> Languages { get; set; } = new(0);
}
