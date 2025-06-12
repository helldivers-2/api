namespace Helldivers.Models.ArrowHead.Info;

/// <summary>
/// A region of a planet, containing information about its health and size.
/// </summary>
/// <param name="Index">The index of the planet region</param>
/// <param name="regionIndex">The index of the region</param>
/// <param name="SettingsHash">The ID that identifies the region.</param>
/// <param name="maxHealth">The maximum health of this region.</param>
/// <param name="regionSize">The size of the region.</param>
public sealed record PlanetRegion(
    int Index,
    int regionIndex,
    ulong SettingsHash,
    ulong maxHealth,
    int regionSize
);
