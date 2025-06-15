namespace Helldivers.Models.V1.Planets;

/// <summary>
/// A region on a planet.
///
/// Note that some properties may be unavailable when the region is inactive.
/// </summary>
/// <param name="Id">The identifier of this region.</param>
/// <param name="Name">The name of the region.</param>
/// <param name="Description">A long-form description of the region.</param>
/// <param name="Health">The current health of the region.</param>
/// <param name="MaxHealth">The maximum health of this region.</param>
/// <param name="Size">The size of this region.</param>
/// <param name="RegenPerSecond">The amount of health this region generates when left alone.</param>
/// <param name="AvailabilityFactor">Unknown purpose.</param>
/// <param name="IsAvailable">Whether the region is currently playable(?).</param>
/// <param name="Players">The amount of helldivers currently active in this region.</param>
public record struct Region(
    int Id,
    string Name,
    string? Description,
    long? Health,
    ulong MaxHealth,
    RegionSize Size,
    double? RegenPerSecond,
    double? AvailabilityFactor,
    bool IsAvailable,
    long Players
);
