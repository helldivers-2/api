using Helldivers.Models.ArrowHead.Assignments;

namespace Helldivers.Models.ArrowHead;

/// <summary>
/// Represents an assignment given from Super Earth to the Helldivers.
/// </summary>
/// <param name="Id32">Internal identifier of this assignment.</param>
/// <param name="Progress">A list of numbers, how they represent progress is unknown.</param>
/// <param name="ExpiresIn">The amount of seconds until this assignment expires.</param>
/// <param name="Setting">Contains detailed information on this assignment like briefing, rewards, ...</param>
public sealed record Assignment(
    long Id32,
    List<int> Progress,
    long ExpiresIn,
    Setting Setting
);
