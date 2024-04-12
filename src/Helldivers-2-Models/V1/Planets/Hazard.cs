namespace Helldivers.Models.V1.Planets;

/// <summary>
/// Describes an environmental hazard that can be present on a <see cref="Planet" />.
/// </summary>
/// <param name="Name">The name of this environmental hazard.</param>
/// <param name="Description">The description of the environmental hazard.</param>
public sealed record Hazard(
    string Name,
    string Description
);
