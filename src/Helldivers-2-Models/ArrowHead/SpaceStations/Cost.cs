namespace Helldivers.Models.ArrowHead.SpaceStations;

/// <summary>
/// Represents the "Cost" of a tactical action
/// </summary>
/// <param name="Id"></param>
/// <param name="ItemMixId"></param>
/// <param name="TargetValue"></param>
/// <param name="CurrentValue"></param>
/// <param name="DeltaPerSecond"></param>
/// <param name="MaxDonationAmmount"></param>
/// <param name="MaxDonationPeriodSeconds"></param>
public sealed record Cost(
    string Id,
    long ItemMixId,
    int TargetValue,
    int CurrentValue,
    int DeltaPerSecond,
    int MaxDonationAmmount,
    int MaxDonationPeriodSeconds
);
