namespace Helldivers.Models.ArrowHead.Assignments;

/// <summary>
/// Represents a task in an <see cref="Assignment" />.
/// It's exact values are not known, therefor little of it's purpose is clear.
/// </summary>
/// <param name="Type">A numerical value, purpose unknown.</param>
/// <param name="Values">A list of numerical values, purpose unknown.</param>
/// <param name="ValueTypes">A list of numerical values, purpose unknown.</param>
public sealed record Task(
    int Type,
    List<int> Values,
    List<int> ValueTypes
);
