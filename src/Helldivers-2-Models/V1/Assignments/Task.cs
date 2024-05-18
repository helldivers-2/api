namespace Helldivers.Models.V1.Assignments;

/// <summary>
/// Represents a task in an <see cref="Assignment" /> that needs to be completed
/// to finish the assignment. 
/// </summary>
/// <param name="Type">The type of task this represents.</param>
/// <param name="Values">A list of numbers, purpose unknown.</param>
/// <param name="ValueTypes">A list of numbers, purpose unknown</param>
public sealed record Task(
    int Type,
    List<ulong> Values,
    List<ulong> ValueTypes
);
