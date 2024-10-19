﻿using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Dispatch" />.
/// </summary>
public sealed class DispatchMapper
{
    /// <summary>
    /// Maps ArrowHead's <see cref="WarInfo" /> onto V1's <see cref="Dispatch" />es.
    /// </summary>
    public IEnumerable<Dispatch> MapToV1(MappingContext context)
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

            yield return MapToV1(context, translations);
        }
    }

    private Dispatch MapToV1(MappingContext context, Dictionary<string, NewsFeedItem> translations)
    {
        var invariant = translations.Values.First();
        var messages = translations.Select(pair => new KeyValuePair<string, string>(pair.Key, pair.Value.Message));

        return new Dispatch(
            Id: invariant.Id,
            Published: context.RelativeGameStart.AddSeconds(invariant.Published),
            Type: invariant.Type,
            Message: LocalizedMessage.FromStrings(messages)
        );
    }
}
