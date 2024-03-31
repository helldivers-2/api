using Helldivers.Models.Domain.Assignments;

namespace Helldivers.Models.Domain;

/// <summary>
/// Represents an assignment for the entire community.
/// </summary>
/// <param name="Index">The numerical identifier of this assignment.</param>
/// <param name="Expiration">When this assignment will expire.</param>
/// <param name="Title">The title for this assignment.</param>
/// <param name="Description">The long-form description of this assignment.</param>
/// <param name="Summary">A (very) short summary of the assignment.</param>
/// <param name="Reward">The reward when the assignment is completed.</param>
public sealed record Assignment(
    long Index,
    DateTime Expiration,
    string Title,
    string Description,
    string Summary,
    Reward Reward
// TODO: map tasks.
);
