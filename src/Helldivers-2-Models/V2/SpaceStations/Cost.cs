namespace Helldivers.Models.V2.SpaceStations;

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
    long TargetValue,
    double CurrentValue,
    long DeltaPerSecond,
    long MaxDonationAmmount,
    long MaxDonationPeriodSeconds
);
