using Helldivers.Models.ArrowHead.Info;

namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// An ongoing event on a planet.
/// </summary>
/// <param name="Id">The unique identifier of this event.</param>
/// <param name="PlanetIndex">The index of the planet.</param>
/// <param name="EventType">A numerical identifier that indicates what type of event this is.</param>
/// <param name="Race">The identifier of the faction that owns the planet currently.</param>
/// <param name="Health">The current health of the planet.</param>
/// <param name="StartTime">When this event started.</param>
/// <param name="ExpireTime">When the event will end.</param>
/// <param name="CampaignId">The unique identifier of a related campaign.</param>
/// <param name="JointOperationIds">A list of identifiers of related joint operations.</param>
public sealed record PlanetEvent(
    int Id,
    int PlanetIndex,
    int EventType,
    int Race,
    long Health,
    long StartTime,
    long ExpireTime,
    long CampaignId,
    List<int> JointOperationIds
);
