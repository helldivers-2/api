using Helldivers.Core.Contracts.Collections;
using Helldivers.Models.V1;

namespace Helldivers.Core.Storage.V1;

/// <inheritdoc cref="IStore{T,TKey}" />
public sealed class AssignmentStore : StoreBase<Assignment, long>
{
    /// <inheritdoc />
    protected override bool GetAsyncPredicate(Assignment assignment, long key)
        => assignment.Id == key;
}
