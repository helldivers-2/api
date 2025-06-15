namespace Helldivers.Models.V1.Planets;

/// <summary>
/// A region on a planet.
///
/// The <c>Name</c> and <c>Description</c> fields may be empty when the underlying data store doesn't contain information on them.
/// This is typically when ArrowHead adds new regions that aren't updated in the data store (helldivers-2/json) yet.
///
/// Note that some properties may be unavailable when the region is inactive.
/// </summary>
/// <param name="Id">The identifier of this region.</param>
/// <param name="Hash">The underlying hash identifier of ArrowHead.</param>
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
    ulong Hash,
    string? Name,
    string? Description,
    long? Health,
    ulong MaxHealth,
    RegionSize Size,
    double? RegenPerSecond,
    double? AvailabilityFactor,
    bool IsAvailable,
    long Players
);
