using Helldivers.Models.Domain.Localization;
using Helldivers.Models.V1.Assignments;
using Task = Helldivers.Models.V1.Assignments.Task;

namespace Helldivers.Models.V1;

/// <summary>
/// Represents an assignment given by Super Earth to the community.
/// This is also known as 'Major Order's in the game.
/// </summary>
/// <param name="Id">The unique identifier of this assignment.</param>
/// <param name="Progress">A list of numbers, how they represent progress is unknown.</param>
/// <param name="Title">The title of the assignment.</param>
/// <param name="Briefing">A long form description of the assignment, usually contains context.</param>
/// <param name="Description">A very short summary of the description.</param>
/// <param name="Tasks">A list of tasks that need to be completed for this assignment.</param>
/// <param name="Reward">The reward for completing the assignment.</param>
public sealed record Assignment(
    long Id,
    List<int> Progress,
    LocalizedMessage Title,
    LocalizedMessage Briefing,
    LocalizedMessage Description,
    List<Task> Tasks,
    Reward Reward
);
