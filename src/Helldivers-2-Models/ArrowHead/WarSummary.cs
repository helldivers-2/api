using Helldivers.Models.ArrowHead.Summary;
using System.Text.Json.Serialization;

namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Gets general statistics about the galaxy and specific planets.
/// </summary>
public sealed record WarSummary
{
    /// <summary>
    /// Contains galaxy wide statistics aggregated from all planets.
    /// </summary>
    [JsonPropertyName("galaxy_stats")]
    public GalaxyStats GalaxyStats { get; set; } = null!;

    /// <summary>
    /// Contains statistics for specific planets.
    /// </summary>
    [JsonPropertyName("planets_stats")]
    public List<PlanetStats> PlanetsStats { get; set; } = null!;
}
