namespace Helldivers.Models.V1;

public record Planet(
    int Index,
    string Name,
    string Sector,
    long Hash,
    Position Position,
    List<int> Waypoints,
    long MaxHealth,
    long Health,
    bool Disabled,
    string InitialOwner,
    string CurrentOwner,
    double RegenPerSecond,
    long Players,
    Statistics Statistics
);
