namespace Helldivers.Models.ArrowHead.Info;

/// <summary>
/// A region of a planet, containing information about its health and size.
/// </summary>
/// <param name="PlanetIndex">The index of the region's planet</param>
/// <param name="RegionIndex">The index of the region</param>
/// <param name="SettingsHash">The ID that identifies the region in the JSON files.</param>
/// <param name="MaxHealth">The maximum health of this region.</param>
/// <param name="RegionSize">The size of the region.</param>
public sealed record PlanetRegion(
    int PlanetIndex,
    int RegionIndex,
    ulong SettingsHash,
    ulong MaxHealth,
    int RegionSize
);
