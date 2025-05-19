using Helldivers.Sync.Hosted;
using Helldivers.Sync.Services;
using System.Globalization;

namespace Helldivers.API.Middlewares;

/// <summary>
/// Automatically appends the `Etag` header to responses.
/// </summary>
public sealed class EtagMiddleware(ILogger<EtagMiddleware> logger, ArrowHeadSyncService arrowHead) : IMiddleware
{
    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (arrowHead.LastUpdated is { } lastUpdated)
        {
            var key = $"{lastUpdated.Subtract(DateTime.UnixEpoch).TotalMilliseconds:0}";
            context.Response.Headers.ETag = key;

            var isModifiedSince = context.Request.Headers.IfModifiedSince.Any(value =>
            {
                if (DateTime.TryParseExact(value, "R", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                        out var date))
                {
                    if (date >= lastUpdated)
                    {
                        return false;
                    }
                }

                return true;
            }) || context.Request.Headers.IfModifiedSince.Count == 0;

            if (isModifiedSince is false)
            {
                context.Response.StatusCode = StatusCodes.Status304NotModified;
                return;
            }
        }

        await next(context);
    }
}
