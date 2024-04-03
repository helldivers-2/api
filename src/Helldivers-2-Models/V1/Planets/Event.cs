namespace Helldivers.Models.V1;

public sealed record Event(
    int Id,
    int EventType,
    string Faction,
    long Health,
    DateTime StartTime,
    DateTime EndTime,
    long CampaignId,
    List<int> JointOperationIds
);
