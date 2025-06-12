namespace Helldivers.Models.V1.Planets;

public record struct Region(
    string Name,
    string Description,
    ulong MaxHealth,
    RegionSize Size,
    double? RegenPerSecond,
    double? AvailabilityFactor,
    bool IsAvailable,
    long Players
);
