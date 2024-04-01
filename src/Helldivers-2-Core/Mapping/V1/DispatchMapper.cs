using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

public sealed class DispatchMapper
{
    public IEnumerable<Dispatch> MapToV1(WarInfo info, Dictionary<string, List<NewsFeedItem>> items)
    {
        // Get a list of all items across all translations.
        var invariants = items
            .SelectMany(pair => pair.Value)
            .DistinctBy(assignment => assignment.Id);

        foreach (var item in invariants)
        {
            // Build a dictionary of all translations for this item
            var translations = items.Select(pair =>
                new KeyValuePair<string, NewsFeedItem?>(
                    pair.Key,
                    pair.Value.FirstOrDefault(a => a.Id == item.Id)
                )
            ).Where(pair => pair.Value is not null)
            .ToDictionary(pair => pair.Key, pair => pair.Value!);

            yield return MapToV1(info, translations);
        }
    }

    public Dispatch MapToV1(WarInfo info, Dictionary<string, NewsFeedItem> translations)
    {
        var invariant = translations.Values.First();
        var messages = translations.Select(pair => new KeyValuePair<string, string>(pair.Key, pair.Value.Message));

        return new Dispatch(
            Id: invariant.Id,
            Published: DateTime.UnixEpoch.AddSeconds(info.StartDate + invariant.Published),
            Type: invariant.Type,
            Message: LocalizedMessage.FromStrings(messages)
        );
    }
}
