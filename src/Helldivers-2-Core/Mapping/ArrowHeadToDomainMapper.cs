using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Handles mapping models from ArrowHead's API (like <see cref="WarInfo" />) to domain models (like <see cref="GalacticWar" />).
/// </summary>
public static class ArrowHeadToDomainMapper
{
    public static GalacticWar MapToDomain(
        string season,
        WarInfo warInfo,
        Dictionary<string, WarStatus> warStatus,
        Dictionary<string, List<NewsFeedItem>> feed,
        Dictionary<string, List<Assignment>> assignments
    )
    {
        throw new NotImplementedException();
    }
}
