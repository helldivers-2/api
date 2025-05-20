namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Contains information of ongoing campaigns.
/// </summary>
/// <param name="Id">The identifier of this campaign.</param>
/// <param name="PlanetIndex">The <see cref="PlanetStatus.Index" /> of the planet this campaign refers to.</param>
/// <param name="Type">A numerical type, indicates the type of campaign (see helldivers-2/json).</param>
/// <param name="Count">A numerical count, the amount of campaigns the planet has seen.</param>
/// <param name="Race">A numerical race, the race of the planet this campaign refers to.</param>
public sealed record Campaign(
    int Id,
    int PlanetIndex,
    int Type,
    ulong Count,
    int Race
);
