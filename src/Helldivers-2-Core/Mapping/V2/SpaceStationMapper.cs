using Helldivers.Models.V1;
using Helldivers.Models.V2;
using Helldivers.Models.V2.SpaceStations;

namespace Helldivers.Core.Mapping.V2;

/// <summary>
/// Handles mapping for <see cref="SpaceStation" />.
/// </summary>
public sealed class SpaceStationMapper
{
    /// <summary>
    /// Maps all space stations in the <see cref="MappingContext" />.
    /// </summary>
    /// <param name="context">The mapping context containing the invariant war status and other relevant data.</param>
    /// <param name="planets">The list of planets to map with.</param>
    /// <returns>An enumerable list of space stations mapped to the V1 model.</returns>
    public IEnumerable<SpaceStation> MapToV2(MappingContext context, List<Planet> planets)
    {
        // Get a list of all assignments across all translations.
        var invariants = context.SpaceStations
            .SelectMany(pair => pair.Value)
            .DistinctBy(spaceStation => spaceStation.Id32);

        foreach (var spaceStation in invariants)
        {
            // Build a dictionary of all translations for this assignment
            var translations = context.SpaceStations.Select(pair =>
                    new KeyValuePair<string, Models.ArrowHead.SpaceStation?>(
                        pair.Key,
                        pair.Value.FirstOrDefault(a => a.Id32 == spaceStation.Id32)
                    )
                ).Where(pair => pair.Value is not null)
                .ToDictionary(pair => pair.Key, pair => pair.Value!);

            yield return Map(translations, context, planets);
        }
    }

    private SpaceStation Map(Dictionary<string, Models.ArrowHead.SpaceStation> translations, MappingContext context, List<Planet> planets)
    {
        var invariant = translations.First().Value;
        var planet = planets.First(p => p.Index == invariant.PlanetIndex);

        return new SpaceStation(
            Id32: invariant.Id32,
            Planet: planet,
            ElectionEnd: context.RelativeGameStart.AddSeconds(invariant.CurrentElectionEndWarTime),
            Flags: invariant.Flags,
            TacticalActions: invariant.TacticalActions.Select(rawAction => MapTacticalAction(context, rawAction)).ToList()
        );
    }

    private TacticalAction MapTacticalAction(MappingContext context, Helldivers.Models.ArrowHead.SpaceStations.TacticalAction raw)
    {
        return new TacticalAction(
            Id32: raw.Id32,
            MediaId32: raw.MediaId32,
            Name: raw.Name,
            Description: raw.Description,
            StrategicDescription: raw.StrategicDescription,
            Status: raw.Status,
            StatusExpire: context.RelativeGameStart.AddSeconds(raw.StatusExpireAtWarTimeSeconds),
            Costs: raw.Cost.Select(MapCost).ToList(),
            EffectIds: raw.EffectIds
        );
    }

    private Cost MapCost(Models.ArrowHead.SpaceStations.Cost cost)
    {
        return new Cost(
            Id: cost.Id,
            ItemMixId: cost.ItemMixId,
            TargetValue: cost.TargetValue,
            CurrentValue: cost.CurrentValue,
            DeltaPerSecond: cost.DeltaPerSecond,
            MaxDonationAmmount: cost.MaxDonationAmmount,
            MaxDonationPeriodSeconds: cost.MaxDonationPeriodSeconds
        );
    }
}
