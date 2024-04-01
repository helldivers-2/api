namespace Helldivers.Models.V1.Assignments;

/// <summary>
/// The reward for completing an <see cref="Assignment" />.
/// </summary>
/// <param name="Type">The type of reward (medals, super credits, ...).</param>
/// <param name="Amount">The amount of <see cref="Type" /> that will be awarded.</param>
public sealed record Reward(
    int Type, // TODO: map to enum
    int Amount
);
