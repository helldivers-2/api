namespace Helldivers.API.Configuration;

/// <summary>
/// Contains configuration for the authentication functionality of the API.
/// </summary>
public sealed class AuthenticationConfiguration
{
    /// <summary>
    /// Whether the API authentication is enabled or disabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// A list of valid issuers of authentication tokens.
    /// </summary>
    public List<string> ValidIssuers { get; set; } = [];

    /// <summary>
    /// A list of valid audiences for said authentication tokens.
    /// </summary>
    public List<string> ValidAudiences { get; set; } = [];

    /// <summary>
    /// A string containing a base64 encoded secret used for signing and verifying authentication tokens.
    /// </summary>
    public string SigningKey { get; set; } = null!;
}
