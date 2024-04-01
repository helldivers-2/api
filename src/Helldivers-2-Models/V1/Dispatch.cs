using Helldivers.Models.Domain.Localization;

namespace Helldivers.Models.V1;

/// <summary>
/// A message from high command to the players, usually updates on the status of the war effort.
/// </summary>
/// <param name="Id">The unique identifier of this dispatch.</param>
/// <param name="Published">When the dispatch was published.</param>
/// <param name="Type">The type of dispatch, purpose unknown.</param>
/// <param name="Message">The message this dispatch represents.</param>
public sealed record Dispatch(
    int Id,
    DateTime Published,
    int Type,
    LocalizedMessage Message
);
