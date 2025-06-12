using Helldivers.Models.ArrowHead.Info;

namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents the 'current' status of a planet's region in the galactic war.
/// </summary>
/// <param name="PlanetIndex">The identifier of the <see cref="PlanetInfo"/> this region is on.</param>
/// <param name="RegionIndex">The identifier of the <see cref="Info.PlanetRegion" /> this data refers to.</param>
/// <param name="Owner">The current faction that controls the region.</param>
/// <param name="Health">The current health / liberation of the region.</param>
/// <param name="RegerPerSecond">If left alone, how much the health of the region would regenerate.</param>
/// <param name="AvailabilityFactor">Unknown purpose.</param>
/// <param name="IsAvailable">Whether this region is currently available to play on(?).</param>
/// <param name="Players">The amount of helldivers currently active on this planet.</param>
public sealed record PlanetRegion(
    int PlanetIndex,
    int RegionIndex,
    int Owner,
    long Health,
    int RegerPerSecond,
    double AvailabilityFactor,
    bool IsAvailable,
    long Players
);
