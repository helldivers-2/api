using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

public class CampaignMapper
{
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
