namespace Helldivers.Models.ArrowHead.Info;

/// <summary>
/// Represents a set of coordinates returned by ArrowHead's API.
/// </summary>
/// <param name="X">The X coordinate</param>
/// <param name="Y">The Y coordinate</param>
public sealed record PlanetCoordinates(double X, double Y);
