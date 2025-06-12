namespace Helldivers.Models.V1.Planets;

public record struct Region(
    /// <summary>
    /// The name of the region.
    /// </summary>
    string Name,
    /// <summary>
    /// The description of the region, if any is available.
    /// </summary>
    string? Description
);
