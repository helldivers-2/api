using Helldivers.API.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Helldivers.API.Controllers;

/// <summary>
/// Controller class that's only available in development, contains local debugging endpoints etc.
/// </summary>
public static class DevelopmentController
{
    private static readonly JwtSecurityTokenHandler TokenHandler = new();

    /// <summary>
    /// Creates a JWT token for the given <paramref name="name" /> and <paramref name="limit" />.
    /// </summary>
    public static IResult CreateToken([FromQuery] string name, [FromQuery] int limit, [FromServices] IOptions<ApiConfiguration> options)
    {
        var key = new SymmetricSecurityKey(Convert.FromBase64String(options.Value.Authentication.SigningKey));
        var token = new JwtSecurityToken(
            issuer: options.Value.Authentication.ValidIssuers.First(),
            audience: options.Value.Authentication.ValidAudiences.First(),
            claims: [
                new Claim("sub", name),
                new Claim("nbf", $"{DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds:0}"),
                new Claim("iat", $"{DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds:0}"),
                new Claim("exp", $"{DateTime.UtcNow.AddDays(30).Subtract(DateTime.UnixEpoch).TotalSeconds:0}"),
                new Claim("RateLimit", $"{limit}")
            ],
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var jwt = TokenHandler.WriteToken(token);
        return Results.Ok(jwt);
    }
}
