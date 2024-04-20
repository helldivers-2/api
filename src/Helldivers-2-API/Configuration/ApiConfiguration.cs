namespace Helldivers.API.Configuration;

/// <summary>
/// Contains configuration of the API.
/// </summary>
public sealed class ApiConfiguration
{
    /// <summary>
    /// The amount of requests that can be made within the time limit.
    /// </summary>
    public int RateLimit { get; set; }

    /// <summary>
    /// The time before the rate limit resets (in seconds).
    /// </summary>
    public int RateLimitWindow { get; set; }

    /// <summary>
    /// Contains the <see cref="AuthenticationConfiguration" /> for the API.
    /// </summary>
    public AuthenticationConfiguration Authentication { get; set; } = null!;
}
