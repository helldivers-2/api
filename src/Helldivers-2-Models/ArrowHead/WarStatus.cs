using Helldivers.Models.ArrowHead.Status;

namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Represents a snapshot of the current status of the galactic war.
/// </summary>
/// <param name="WarId">The war season this snapshot refers to.</param>
/// <param name="Time">The time this snapshot was taken.</param>
/// <param name="ImpactMultiplier">This is the factor by which XP at the end of a mission is multiplied to calculate the impact on liberation</param>
/// <param name="StoryBeatId32">Internal identifier, purpose unknown.</param>
/// <param name="PlanetStatus">A list of statuses for planets.</param>
/// <param name="PlanetAttacks">A list of attacks currently ongoing.</param>
/// <param name="Campaigns">A list of ongoing campaigns in the galactic war.</param>
public sealed record WarStatus(
    int WarId,
    long Time,
    double ImpactMultiplier,
    long StoryBeatId32,
    List<PlanetStatus> PlanetStatus,
    List<PlanetAttack> PlanetAttacks,
    List<Campaign> Campaigns,
    // TODO CommunityTargets
    List<JointOperation> JointOperations,
    List<PlanetEvent> PlanetEvents
// TODO PlanetActiveEffects
// TODO activeElectionPolicyEffects
// TODO globalEvents
// TODO superEarthWarResults
);
