namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Contains information of ongoing campaigns.
/// </summary>
/// <param name="Id">The identifier of this campaign.</param>
/// <param name="PlanetIndex">The <see cref="PlanetStatus.Index" /> of the planet this campaign refers to.</param>
/// <param name="Type">A numerical type, purpose unknown.</param>
/// <param name="Count">A numerical count, unknown what it counts.</param>
public sealed record Campaign(
    int Id,
    int PlanetIndex,
    int Type,
    int Count
);
