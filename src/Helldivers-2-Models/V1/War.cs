namespace Helldivers.Models.V1;

/// <summary>
/// Global information of the ongoing war.
/// </summary>
/// <param name="Started">When this war was started.</param>
/// <param name="Ended">When this war will end (or has ended).</param>
/// <param name="Now">The time the snapshot of the war was taken, also doubles as the timestamp of which all other data dates from.</param>
/// <param name="ClientVersion">The minimum game client version required to play in this war.</param>
/// <param name="Factions">A list of factions currently involved in the war.</param>
/// <param name="ImpactMultiplier">A fraction used to calculate the impact of a mission on the war effort.</param>
/// <param name="Statistics">The statistics available for the galaxy wide war effort.</param>
public record War(
    DateTime Started,
    DateTime Ended,
    DateTime Now,
    string ClientVersion,
    List<string> Factions,
    double ImpactMultiplier,
    Statistics Statistics
);
