namespace Helldivers.Models.Domain.War;

/// <summary>
/// Represents a position on the galactic war map.
/// </summary>
/// <param name="X">The X coordinate.</param>
/// <param name="Y">The Y coordinate.</param>
public record struct Position(
    double X,
    double Y
);
