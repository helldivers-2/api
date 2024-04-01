namespace Helldivers.Models.V1;

/// <summary>
/// Contains statistics of missions, kills, success rate etc.
/// </summary>
/// <param name="MissionsWon">The amount of missions won.</param>
/// <param name="MissionsLost">The amount of missions lost.</param>
/// <param name="MissionTime">The total amount of time spent planetside (in seconds).</param>
/// <param name="TerminidKills">The total amount of bugs killed since start of the season.</param>
/// <param name="AutomatonKills">The total amount of automatons killed since start of the season.</param>
/// <param name="IlluminateKills">The total amount of Illuminate killed since start of the season.</param>
/// <param name="BulletsFired">The total amount of bullets fired</param>
/// <param name="BulletsHit">The total amount of bullets hit</param>
/// <param name="TimePlayed">The total amount of time played (including off-planet) in seconds.</param>
/// <param name="Deaths">The amount of casualties on the side of humanity.</param>
/// <param name="Revives">The amount of revives(?).</param>
/// <param name="Friendlies">The amount of friendly fire casualties.</param>
/// <param name="MissionSuccessRate">A percentage indicating how many started missions end in success.</param>
/// <param name="Accuracy">A percentage indicating average accuracy of Helldivers.</param>
public sealed record Statistics(
    ulong MissionsWon,
    ulong MissionsLost,
    ulong MissionTime,
    ulong TerminidKills,
    ulong AutomatonKills,
    ulong IlluminateKills,
    ulong BulletsFired,
    ulong BulletsHit,
    ulong TimePlayed,
    ulong Deaths,
    ulong Revives,
    ulong Friendlies,
    ulong MissionSuccessRate,
    ulong Accuracy
);
