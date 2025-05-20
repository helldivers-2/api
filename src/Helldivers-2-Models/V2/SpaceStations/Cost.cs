namespace Helldivers.Models.V2.SpaceStations;

public sealed record Cost(
    string Id,
    long ItemMixId,
    long TargetValue,
    double CurrentValue,
    long DeltaPerSecond,
    long MaxDonationAmmount,
    long MaxDonationPeriodSeconds
);
