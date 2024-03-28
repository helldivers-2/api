using Helldivers.Models.ArrowHead.Info;

namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Represents mostly static information of the current galactic war.
/// </summary>
/// <param name="WarId">The identifier of the war season this <see cref="WarInfo" /> represents.</param>
/// <param name="StartDate">A unix timestamp (in seconds) when this season started.</param>
/// <param name="EndDate">A unix timestamp (in seconds) when this season will end.</param>
/// <param name="MinimumClientVersion">A version string indicating the minimum game client version the API supports.</param>
/// <param name="PlanetInfos">A list of planets involved in this season's war.</param>
/// <param name="HomeWorlds">A list of homeworlds for the races (factions) involved in this war.</param>
public sealed record WarInfo(
    int WarId,
    long StartDate,
    long EndDate,
    string MinimumClientVersion,
    List<PlanetInfo> PlanetInfos,
    List<HomeWorld> HomeWorlds
    // TODO: capitalInfo's
    // TODO planetPermanentEffects
);
