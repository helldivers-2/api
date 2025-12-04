using Helldivers.Models;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Campaign" />.
/// </summary>
public class CampaignMapper
{
    /// <summary>
    /// Maps all <see cref="Campaign" /> in the current context. 
    /// </summary>
    public IEnumerable<Campaign> MapToV1(MappingContext context, List<Planet> planets)
    {
        foreach (var campaign in context.InvariantWarStatus.Campaigns)
        {
            var result = MapToV1(campaign, planets);
            
            if (result is not null)
                yield return result;
        }
    }

    /// <summary>
    /// Maps ArrowHead's <see cref="Models.ArrowHead.Status.Campaign" /> onto V1's.
    /// </summary>
    private Campaign? MapToV1(Models.ArrowHead.Status.Campaign campaign, List<Planet> planets)
    {
        var planet = planets.FirstOrDefault(p => p.Index == campaign.PlanetIndex);
        Static.Factions.TryGetValue(campaign.Race, out var currentOwner);

        if (planet is null)
            return null;

        return new Campaign(
            Id: campaign.Id,
            Planet: planet,
            Type: campaign.Type,
            Count: campaign.Count,
            Faction: currentOwner ?? string.Empty
        );
    }
}
