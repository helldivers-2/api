namespace Helldivers.Models.V1;

/// <summary>
/// Represents an ongoing campaign on a planet.
/// </summary>
/// <param name="Id">The unique identifier of this <see cref="Campaign" />.</param>
/// <param name="Planet">The planet on which this campaign is being fought.</param>
/// <param name="Type">The type of campaign, this should be mapped onto an enum.</param>
/// <param name="Count">Indicates how many campaigns have already been fought on this <see cref="Planet" />.</param>
/// <param name="Faction">The faction that is currently fighting this campaign.</param>
public record Campaign(
    int Id,
    Planet Planet,
    int Type, // TODO: map to enum
    ulong Count,
    string Faction
);
