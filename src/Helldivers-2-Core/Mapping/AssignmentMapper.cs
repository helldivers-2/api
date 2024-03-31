using Helldivers.Models.Domain;
using Helldivers.Models.Domain.Assignments;
using ArrowHeadAssignment = Helldivers.Models.ArrowHead.Assignment;
using ArrowHeadReward = Helldivers.Models.ArrowHead.Assignments.Reward;

namespace Helldivers.Core.Mapping;

/// <summary>
/// Handles mapping of <see cref="ArrowHeadAssignment" /> to <see cref="Assignment" />.
/// </summary>
public static class AssignmentMapper
{
    /// <summary>
    /// Maps <see cref="ArrowHeadAssignment" /> to <see cref="Assignment" />.
    /// </summary>
    public static Assignment MapToDomain(ArrowHeadAssignment assignment)
    {
        return new Assignment(
            Index: assignment.Id32,
            Expiration: DateTime.UnixEpoch.AddSeconds(assignment.ExpiresIn),
            Title: assignment.Setting.OverrideTitle,
            Description: assignment.Setting.TaskDescription,
            Summary: assignment.Setting.OverrideBrief,
            Reward: MapToDomain(assignment.Setting.Reward)
        );
    }

    /// <summary>
    /// Maps <see cref="Reward" />
    /// </summary>
    public static Reward MapToDomain(ArrowHeadReward reward)
    {
        var type = reward.Type switch
        {
            1 => RewardType.Medals,
            _ => throw new InvalidCastException($"Cannot cast '{reward.Type}' to known reward type")
        };

        return new Reward(
            Type: type,
            Amount: reward.Amount
        );
    }
}
