using Helldivers.API.Configuration;
using Helldivers.API.Metrics;
using Microsoft.Extensions.Options;

namespace Helldivers.API.Middlewares;

/// <summary>
/// Handles closing connections from blacklisted clients that violate ToS.
/// </summary>
public sealed class BlacklistMiddleware(IOptions<ApiConfiguration> options) : IMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var client = ClientMetric.GetClientName(context);
        if (options.Value.Blacklist.Contains(client, StringComparison.InvariantCultureIgnoreCase))
        {
            // don't send response, only wastes more bytes.
            context.Abort();
            return;
        }

        await next(context);
    }
}
