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
    /// A comma separated list of clients that are (temporarily) blacklisted from making requests.
    /// </summary>
    public string Blacklist { get; set; } = string.Empty;

    /// <summary>
    /// Whether X-Super-Client and X-Super-Contact headers are validated.
    /// </summary>
    public bool ValidateClients { get; set; } = true;

    /// <summary>
    /// Contains the <see cref="AuthenticationConfiguration" /> for the API.
    /// </summary>
    public AuthenticationConfiguration Authentication { get; set; } = null!;
}
