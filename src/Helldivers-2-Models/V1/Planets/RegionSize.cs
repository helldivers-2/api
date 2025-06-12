namespace Helldivers.Models.V1.Planets;

/// <summary>
/// Indicates what size a <see cref="Region" /> has.
/// </summary>
public enum RegionSize
{
    /// <summary>
    /// The region is a settlement.
    /// </summary>
    Settlement = 0,
    /// <summary>
    ///  The region is a town.
    /// </summary>
    Town = 1,
    /// <summary>
    ///  The region is a city.
    /// </summary>
    City = 2,
    /// <summary>
    /// The region is a megacity
    /// </summary>
    MegaCity = 3,
}
