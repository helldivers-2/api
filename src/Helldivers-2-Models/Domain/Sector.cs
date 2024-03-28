namespace Helldivers.Models.Domain;

/// <summary>
/// Represents a sector of planets.
/// </summary>
/// <param name="Index">The numerical ID of this sector.</param>
/// <param name="Name">The human-readable name of this sector</param>
/// <param name="Planets">A list of planets in this sector.</param>
public sealed record Sector(
    int Index,
    string Name,
    List<Planet> Planets
);
