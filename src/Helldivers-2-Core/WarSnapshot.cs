using Helldivers.Core.Mapping;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Helldivers.Core;

/// <summary>
/// Represents a snapshot of the current galactic war.
/// It provides access to all information synchronized from the ArrowHead API,
/// but also keeps track of exceptions that might have occured.
/// </summary>
public sealed class WarSnapshot
{
    /// <summary>
    /// The identifier of the season that is currently active.
    /// </summary>
    public string? Season { get; set; }

    /// <summary>
    /// The raw <see cref="WarInfo" /> as returned by the ArrowHead API.
    /// </summary>
    public WarInfo? ArrowHeadWarInfo { get; set; }

    /// <summary>
    /// A dictionary of <see cref="WarStatus" /> (the value) for languages (the keys) as returned by the ArrowHead API.
    /// </summary>
    public CultureDictionary<WarStatus>? ArrowHeadWarStatus { get; set; }

    /// <summary>
    /// Gets a snapshot of the current <see cref="WarSummary" /> statistics.
    /// </summary>
    public WarSummary? ArrowHeadWarSummary { get; set; }

    /// <summary>
    /// A dictionary of <see cref="NewsFeedItem" /> (the value) for languages (the keys) as returned by the ArrowHead API.
    /// </summary>
    public CultureDictionary<List<NewsFeedItem>>? ArrowHeadNewsFeed { get; set; }

    /// <summary>
    /// A dictionary of <see cref="Assignment" /> (the value) for languages (the keys) as returned by the ArrowHead API.
    /// </summary>
    public CultureDictionary<List<Assignment>>? ArrowHeadAssignments { get; set; }

    /// <summary>
    /// Current denormalized galactic war information.
    /// </summary>
    public GalacticWar? GalacticWar { get; set; }

    /// <summary>
    /// Called after a sync to update the currently active snapshot information.
    /// </summary>
    public void UpdateSnapshot(
        string season,
        WarInfo warInfo,
        WarSummary summary,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<Assignment>> assignments
    )
    {
        Season = season;
        ArrowHeadWarInfo = warInfo;
        ArrowHeadWarStatus = new (warStatus);
        ArrowHeadWarSummary = summary;
        ArrowHeadNewsFeed = new (feed);
        ArrowHeadAssignments = new (assignments);

        GalacticWar = GalacticWarMapper.MapToDomain(season, warInfo, summary, warStatus, feed, assignments);
    }
}
