using Helldivers.Sync.Hosted;

namespace Helldivers.Sync.Exceptions;

/// <summary>
/// Thrown by the synchronization services if ArrowHead returns empty translation messages.
/// This *should* trigger a re-sync by the <see cref="ArrowHeadSyncService" />
/// </summary>
/// <param name="message"></param>
public class EmptyMessageException(string message) : Exception(message)
{
    //
}
