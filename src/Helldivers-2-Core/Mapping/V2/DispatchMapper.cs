using Helldivers.Core.Hdml;
using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V2;

namespace Helldivers.Core.Mapping.V2;

/// <summary>
/// Handles mapping for <see cref="Dispatch" />.
/// </summary>
public sealed class DispatchMapper(HdmlParser parser)
{
    /// <summary>
    /// Maps ArrowHead's <see cref="WarInfo" /> onto V2's <see cref="Dispatch" />es.
    /// </summary>
    public IEnumerable<Dispatch> MapToV2(MappingContext context)
    {
        // Get a list of all items across all translations.
        var invariants = context.NewsFeeds
            .SelectMany(pair => pair.Value)
            .DistinctBy(assignment => assignment.Id);

        foreach (var item in invariants)
        {
            // Build a dictionary of all translations for this item
            var translations = context.NewsFeeds.Select(pair =>
                    new KeyValuePair<string, NewsFeedItem?>(
                        pair.Key,
                        pair.Value.FirstOrDefault(a => a.Id == item.Id)
                    )
                ).Where(pair => pair.Value is not null)
                .ToDictionary(pair => pair.Key, pair => pair.Value!);

            yield return MapToV2(context, translations);
        }
    }

    private Dispatch MapToV2(MappingContext context, Dictionary<string, NewsFeedItem> translations)
    {
        var invariant = translations.Values.First();
        var messages = translations.Select(pair =>
            new KeyValuePair<string, string>(pair.Key, parser.Compile(pair.Value.Message)));

        return new Dispatch(
            Id: invariant.Id,
            Published: context.RelativeGameStart.AddSeconds(invariant.Published),
            Type: invariant.Type,
            Message: LocalizedMessage.FromStrings(messages)
        );
    }
}
