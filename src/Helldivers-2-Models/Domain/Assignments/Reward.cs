namespace Helldivers.Models.Domain.Assignments;

/// <summary>
/// Represents the reward (and amount) 
/// </summary>
/// <param name="Type">The type of reward (medals, super credits, ...).</param>
/// <param name="Amount">The amount of <paramref name="Type" /> that will be rewarded.</param>
public sealed record Reward(
    RewardType Type,
    int Amount
);
