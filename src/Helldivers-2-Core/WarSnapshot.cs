using Helldivers.Core.Localization;
using Helldivers.Core.Mapping;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;
using ArrowHeadAssignment = Helldivers.Models.ArrowHead.Assignment;
using Assignment = Helldivers.Models.Domain.Assignment;

namespace Helldivers.Core;

/// <summary>
/// Represents a snapshot of the current galactic war.
/// It provides access to all information synchronized from the ArrowHead API,
/// but also keeps track of exceptions that might have occured.
/// </summary>
[Obsolete("Use IStore<T> and IStore<T, TKey>")]
public sealed class WarSnapshot
{
    /// <summary>
    /// The identifier of the season that is currently active.
    /// </summary>
    public string? Season { get; set; }

    /// <summary>
    /// Current denormalized galactic war information.
    /// </summary>
    public GalacticWar? GalacticWar { get; set; }

    /// <summary>
    /// A dictionary of <see cref="NewsItem" /> items.
    /// </summary>
    public CultureDictionary<List<NewsItem>>? NewsFeed { get; set; }

    /// <summary>
    /// A dictionary of <see cref="Assignment" />s.
    /// </summary>
    public CultureDictionary<List<Assignment>>? Assignments { get; set; }

    /// <summary>
    /// Called after a sync to update the currently active snapshot information.
    /// </summary>
    public void UpdateSnapshot(
        string season,
        WarInfo warInfo,
        WarSummary summary,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<ArrowHeadAssignment>> assignments
    )
    {
        Season = season;

        GalacticWar = GalacticWarMapper.MapToDomain(season, warInfo, summary, warStatus, feed, assignments);
        NewsFeed = new(feed.Select(pair =>
        {
            var values = pair
                .Value
                .Select(item => NewsItemMapper.MapToDomain(item, GalacticWar))
                .OrderByDescending(item => item.PublishedAt)
                .ToList();

            return new KeyValuePair<string, List<NewsItem>>(pair.Key, values);
        }));
        Assignments = new(assignments.Select(pair =>
        {
            var values = pair
                .Value
                .Select(AssignmentMapper.MapToDomain)
                .OrderByDescending(item => item.Index)
                .ToList();

            return new KeyValuePair<string, List<Assignment>>(pair.Key, values);
        }));
    }
}
