namespace Helldivers.Models.V1.Planets;

/// <summary>
/// Represents information about a biome of a planet.
/// </summary>
/// <param name="Name">The name of this biome.</param>
/// <param name="Description">A human-readable description of the biome.</param>
public sealed record Biome(
    string Name,
    string Description
)
{
    /// <summary>
    /// Used when the biome could not be determined.
    /// </summary>
    public static readonly Biome Unknown = new("UNKNOWN BIOME", string.Empty);
};
