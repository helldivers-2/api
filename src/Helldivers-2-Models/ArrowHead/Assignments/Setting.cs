namespace Helldivers.Models.ArrowHead.Assignments;

/// <summary>
/// Contains the details of an <see cref="Assignment" /> like reward and requirements.
/// </summary>
/// <param name="Type">The type of assignment, values unknown at the moment.</param>
/// <param name="OverrideTitle">The title of this assignment.</param>
/// <param name="OverrideBrief">The briefing (description) of this assignment.</param>
/// <param name="TaskDescription">A description of what is expected of Helldivers to complete the assignment.</param>
/// <param name="Tasks">A list of <see cref="Task" />s describing the assignment requirements.</param>
/// <param name="Reward">Contains information on the rewards players willr eceive upon completion.</param>
/// <param name="Flags">Flags, suspected to be a binary OR'd value, purpose unknown.</param>
public sealed record Setting(
    int Type,
    string OverrideTitle,
    string OverrideBrief,
    string TaskDescription,
    List<Task> Tasks,
    Reward Reward,
    int Flags
);
