using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Campaign" />.
/// </summary>
public class CampaignMapper
{
    /// <summary>
    /// Maps ArrowHead's <see cref="Models.ArrowHead.Status.Campaign" /> onto V1's.
    /// </summary>
    public Campaign MapToV1(Models.ArrowHead.Status.Campaign campaign, List<Planet> planets)
    {
        var planet = planets.First(p => p.Index == campaign.PlanetIndex);

        return new Campaign(
            Id: campaign.Id,
            Planet: planet,
            Type: campaign.Type,
            Count: campaign.Count
        );
    }
}
