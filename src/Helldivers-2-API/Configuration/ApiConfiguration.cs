namespace Helldivers.API.Configuration;

/// <summary>
/// Contains configuration of the API.
/// </summary>
public sealed class ApiConfiguration
{
    /// <summary>
    /// Contains the <see cref="AuthenticationConfiguration" /> for the API.
    /// </summary>
    public AuthenticationConfiguration Authentication { get; set; } = null!;
}
