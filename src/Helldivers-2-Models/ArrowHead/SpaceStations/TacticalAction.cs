namespace Helldivers.Models.ArrowHead.SpaceStations;

/// <summary>
/// Represents information of a space station from the 'SpaceStation' endpoint returned by ArrowHead's API.
/// </summary>
/// <param name="Id32"></param>
/// <param name="MediaId32"></param>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="StrategicDescription"></param>
/// <param name="Status"></param>
/// <param name="StatusExpireAtWarTimeSeconds"></param>
/// <param name="Cost"></param>
/// <param name="EffectIds"></param>
/// <param name="ActiveEffectIds"></param>
public sealed record TacticalAction(
    long Id32,
    long MediaId32,
    string Name,
    string Description,
    string StrategicDescription,
    int Status,
    int StatusExpireAtWarTimeSeconds,
    List<Cost> Cost,
    List<int> EffectIds,
    List<int> ActiveEffectIds
);
