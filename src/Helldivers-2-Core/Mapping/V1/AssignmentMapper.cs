using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1;
using Helldivers.Models.V1.Assignments;
using Task = Helldivers.Models.V1.Assignments.Task;

namespace Helldivers.Core.Mapping.V1;

/// <summary>
/// Handles mapping for <see cref="Assignment" />.
/// </summary>
public sealed class AssignmentMapper
{
    /// <summary>
    /// Maps a set of multi-language <see cref="Assignment" />s into a list of <see cref="Assignment" />.
    /// </summary>
    public IEnumerable<Assignment> MapToV1(Dictionary<string, List<Models.ArrowHead.Assignment>> assignments)
    {
        // Get a list of all assignments across all translations.
        var invariants = assignments
            .SelectMany(pair => pair.Value)
            .DistinctBy(assignment => assignment.Id32);

        foreach (var assignment in invariants)
        {
            // Build a dictionary of all translations for this assignment
            var translations = assignments.Select(pair =>
                new KeyValuePair<string, Models.ArrowHead.Assignment?>(
                    pair.Key,
                    pair.Value.FirstOrDefault(a => a.Id32 == assignment.Id32)
                )
            ).Where(pair => pair.Value is not null)
            .ToDictionary(pair => pair.Key, pair => pair.Value!);

            yield return MapToV1(translations);
        }
    }

    /// <summary>
    /// Maps all translations of an <see cref="Assignment" /> into one assignment.
    /// </summary>
    private Assignment MapToV1(Dictionary<string, Models.ArrowHead.Assignment> translations)
    {
        var invariant = translations.First().Value;
        var titles = translations.Select(assignment => new KeyValuePair<string, string>(assignment.Key, assignment.Value.Setting.OverrideTitle));
        var briefings = translations.Select(assignment => new KeyValuePair<string, string>(assignment.Key, assignment.Value.Setting.OverrideBrief));
        var descriptions = translations.Select(assignment => new KeyValuePair<string, string>(assignment.Key, assignment.Value.Setting.TaskDescription));

        return new Assignment(
            Id: invariant.Id32,
            Title: LocalizedMessage.FromStrings(titles),
            Briefing: LocalizedMessage.FromStrings(briefings),
            Description: LocalizedMessage.FromStrings(descriptions),
            Tasks: invariant.Setting.Tasks.Select(MapToV1).ToList(),
            Reward: MapToV1(invariant.Setting.Reward)
        );
    }

    private Task MapToV1(Models.ArrowHead.Assignments.Task task)
    {
        return new Task(
            Type: task.Type,
            Values: task.Values,
            ValueTypes: task.ValueTypes
        );
    }

    private Reward MapToV1(Models.ArrowHead.Assignments.Reward reward)
    {
        return new Reward(
            Type: reward.Type,
            Amount: reward.Amount
        );
    }
}
