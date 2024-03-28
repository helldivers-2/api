using Helldivers.Models.ArrowHead;
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
    public Dictionary<string, WarStatus>? ArrowHeadWarStatus { get; set; }

    /// <summary>
    /// A dictionary of <see cref="NewsFeedItem" /> (the value) for languages (the keys) as returned by the ArrowHead API.
    /// </summary>
    public Dictionary<string, List<NewsFeedItem>>? ArrowHeadNewsFeed { get; set; }

    /// <summary>
    /// A dictionary of <see cref="Assignment" /> (the value) for languages (the keys) as returned by the ArrowHead API.
    /// </summary>
    public Dictionary<string, List<Assignment>>? ArrowHeadAssignments { get; set; }

    public void UpdateSnapshot(
        string season,
        WarInfo warInfo,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<Assignment>> assignments
    )
    {
        Season = season;
        ArrowHeadWarInfo = warInfo;
        ArrowHeadWarStatus = warStatus;
        ArrowHeadNewsFeed = feed;
        ArrowHeadAssignments = assignments;
    }
}
