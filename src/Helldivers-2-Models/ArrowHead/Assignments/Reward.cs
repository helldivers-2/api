namespace Helldivers.Models.ArrowHead.Assignments;

/// <summary>
/// Represents the reward of an <see cref="Assignment" />.
/// </summary>
/// <param name="Type">The type of reward, currently only one value is known: <c>1</c> which represents Medals</param>
/// <param name="Id32">Internal identifier of this <see cref="Reward"/>.</param>
/// <param name="Amount">The amount of <see cref="Type" /> the players will receive upon completion.</param>
public sealed record Reward(
    int Type,
    ulong Id32,
    ulong Amount
);
