namespace Helldivers.Models.V1.SpaceStations;

/// <summary>
/// Represents a tactical action that the Space Station can take.
/// </summary>
public sealed record TacticalAction(
    long Id32,
    long MediaId32,
    string Name,
    string Description,
    string StrategicDescription,
    int Status,
    DateTime StatusExpire,
    List<Cost> Costs,
    List<int> EffectIds
);
