using Helldivers.Models.ArrowHead;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Contains the context of the ArrowHead models currently being mapped.
/// It's main purpose is to provide a convenient way to pass around <b>all</b> ArrowHead data
/// at once without having to pass 7-8 parameters to each function (or when adding more data,
/// having to update all dependent signatures).
/// </summary>
public sealed class MappingContext
{
    /// <summary>The <see cref="WarId" /> currently being mapped.</summary>
    public WarId WarId { get; private init; }

    /// <summary>The <see cref="WarInfo" /> currently being mapped.</summary>
    public WarInfo WarInfo { get; private init; }

    /// <summary>The <see cref="WarStatus" />es currently being mapped.</summary>
    public Dictionary<string, WarStatus> WarStatuses { get; private init; }

    /// <summary>A <see cref="WarStatus" /> that can be used when accessing non-localized data.</summary>
    public WarStatus InvariantWarStatus { get; private init; }

    /// <summary>The <see cref="WarSummary" /> currently being mapped.</summary>
    public WarSummary WarSummary { get; private init; }

    /// <summary>The <see cref="NewsFeedItem" />s currently being mapped.</summary>
    public Dictionary<string, List<NewsFeedItem>> NewsFeeds { get; private init; }

    /// <summary>The <see cref="Assignment" />s currently being mapped.</summary>
    public Dictionary<string, List<Assignment>> Assignments { get; private init; }

    /// <summary>The <see cref="SpaceStation" />s currently being mapped.</summary>
    public Dictionary<string, List<SpaceStation>> SpaceStations { get; private init; }

    /// <summary>
    /// A <see cref="DateTime" /> that represents the 'start' of the time in Helldivers 2.
    /// This accounts for the <see cref="Models.ArrowHead.WarInfo.StartDate" /> and <see cref="GameTimeDeviation" />.
    /// </summary>
    public DateTime RelativeGameStart { get; private init; }

    /// <summary>
    /// There's a deviation between gametime and real world time.
    /// When calculating dates using ArrowHead timestamps, add this to compensate for the deviation.
    /// </summary>
    public TimeSpan GameTimeDeviation { get; private init; }

    /// <summary>Initializes a new <see cref="MappingContext" />.</summary>
    internal MappingContext(WarId warId,
        WarInfo warInfo,
        Dictionary<string, WarStatus> warStatuses,
        WarSummary warSummary,
        Dictionary<string, List<NewsFeedItem>> newsFeeds,
        Dictionary<string, List<Assignment>> assignments,
        Dictionary<string, List<SpaceStation>> spaceStations)
    {
        WarId = warId;
        WarInfo = warInfo;
        WarStatuses = warStatuses;
        WarSummary = warSummary;
        NewsFeeds = newsFeeds;
        Assignments = assignments;
        SpaceStations = spaceStations;

        InvariantWarStatus = warStatuses.FirstOrDefault().Value
                             ?? throw new InvalidOperationException("No warstatus available");


        var gameTime = DateTime.UnixEpoch.AddSeconds(warInfo.StartDate + InvariantWarStatus.Time);
        GameTimeDeviation = TruncateToSeconds(DateTime.UtcNow).Subtract(gameTime);
        RelativeGameStart = DateTime.UnixEpoch.Add(GameTimeDeviation).AddSeconds(warInfo.StartDate);
    }

    /// <summary>
    /// ArrowHead doesn't send timestamps more accurate than seconds, so we truncate our relative time to seconds.
    /// This prevents timestamps for the same value from being different (due to milli/micro second differences).
    /// </summary>
    private static DateTime TruncateToSeconds(DateTime dateTime) => dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
}
