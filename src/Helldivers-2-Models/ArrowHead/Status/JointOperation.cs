namespace Helldivers.Models.ArrowHead.Status;

/// <summary>
/// Represents a joint operation.
/// </summary>
/// <param name="Id"></param>
/// <param name="PlanetIndex"></param>
/// <param name="HqNodeIndex"></param>
public sealed record JointOperation(
    int Id,
    int PlanetIndex,
    int HqNodeIndex
);
