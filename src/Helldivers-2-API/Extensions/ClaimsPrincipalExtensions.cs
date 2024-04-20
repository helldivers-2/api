using System.Security.Claims;

namespace Helldivers.API.Extensions;

/// <summary>
/// Contains extension methods for working with <see cref="ClaimsPrincipal" />s.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Attempts to retrieve the integer value of a <see cref="Claim" /> with type <paramref name="type" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the claim could not be found, or is not a valid integer.</exception>
    public static int GetIntClaim(this ClaimsPrincipal user, string type)
    {
        var claim = user.Claims.FirstOrDefault(c =>
            string.Equals(c.Type, type, StringComparison.InvariantCultureIgnoreCase));

        if (claim is { Value: var str } && int.TryParse(str, out var result))
            return result;

        throw new InvalidOperationException($"Cannot fetch {type} or it is not a valid number");
    }
}
