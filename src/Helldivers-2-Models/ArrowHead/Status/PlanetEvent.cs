namespace Helldivers.Models.ArrowHead.Status;

public sealed record PlanetEvent(
    int Index,
    int PlanetIndex,
    int EventType,
    int Race,
    long Health,
    long StartTime,
    long ExpireTime,
    long CampaignId,
    List<int> JointOperationIds
);
