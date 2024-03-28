namespace Helldivers.Models.ArrowHead.Info;

/// <summary>
/// Represents information about the homeworld(s) of a given race.
/// </summary>
/// <param name="Race">The identifier of the race (faction) this describes the homeworld of.</param>
/// <param name="PlanetIndices">A list of <see cref="PlanetInfo.Index" /> identifiers.</param>
public sealed record HomeWorld(int Race, List<int> PlanetIndices);
