namespace Helldivers.Models.V1.Planets;

/// <summary>
/// An ongoing event on a <see cref="Planet" />.
/// </summary>
/// <param name="Id">The unique identifier of this event.</param>
/// <param name="EventType">The type of event.</param>
/// <param name="Faction">The faction that initiated the event.</param>
/// <param name="Health">The health of the <see cref="Event" /> at the time of snapshot.</param>
/// <param name="MaxHealth">The maximum health of the <see cref="Event" /> at the time of snapshot.</param>
/// <param name="StartTime">When the event started.</param>
/// <param name="EndTime">When the event will end.</param>
/// <param name="CampaignId">The identifier of the <see cref="Campaign" /> linked to this event.</param>
/// <param name="JointOperationIds">A list of joint operation identifier linked to this event.</param>
public sealed record Event(
    int Id,
    int EventType,
    string Faction,
    long Health,
    long MaxHealth,
    DateTime StartTime,
    DateTime EndTime,
    long CampaignId,
    List<int> JointOperationIds
);
