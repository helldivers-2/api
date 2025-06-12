﻿using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1.Planets;

namespace Helldivers.Models.V1;

/// <summary>
/// Contains all aggregated information AH has about a planet.
/// </summary>
/// <param name="Index">The unique identifier ArrowHead assigned to this planet.</param>
/// <param name="Name">The name of the planet, as shown in game.</param>
/// <param name="Sector">The name of the sector the planet is in, as shown in game.</param>
/// <param name="Biome">The biome this planet has.</param>
/// <param name="Hazards">All <see cref="Hazard" />s that are applicable to this planet.</param>
/// <param name="Hash">A hash assigned to the planet by ArrowHead, purpose unknown.</param>
/// <param name="Position">The coordinates of this planet on the galactic war map.</param>
/// <param name="Waypoints">A list of <see cref="Index" /> of all the planets to which this planet is connected.</param>
/// <param name="MaxHealth">The maximum health pool of this planet.</param>
/// <param name="Health">The current planet this planet has.</param>
/// <param name="Disabled">Whether or not this planet is disabled, as assigned by ArrowHead.</param>
/// <param name="InitialOwner">The faction that originally owned the planet.</param>
/// <param name="CurrentOwner">The faction that currently controls the planet.</param>
/// <param name="RegenPerSecond">How much the planet regenerates per second if left alone.</param>
/// <param name="Event">Information on the active event ongoing on this planet, if one is active.</param>
/// <param name="Statistics">A set of statistics scoped to this planet.</param>
/// <param name="Attacking">A list of <see cref="Index" /> integers that this planet is currently attacking.</param>
/// <param name="Regions">All regions on this planet, including their status (if any).</param>
public record Planet(
    int Index,
    LocalizedMessage Name,
    string Sector,
    Biome Biome,
    List<Hazard> Hazards,
    long Hash,
    Position Position,
    List<int> Waypoints,
    long MaxHealth,
    long Health,
    bool Disabled,
    string InitialOwner,
    string CurrentOwner,
    double RegenPerSecond,
    Event? Event,
    Statistics Statistics,
    List<int> Attacking,
    List<Region> Regions
);
