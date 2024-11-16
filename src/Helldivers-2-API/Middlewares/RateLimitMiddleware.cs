using Helldivers.API.Configuration;
using Helldivers.API.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace Helldivers.API.Middlewares;

/// <summary>
/// Handles applying rate limit logic to the API's requests.
/// </summary>
public sealed partial class RateLimitMiddleware(
    ILogger<RateLimitMiddleware> logger,
    IOptions<ApiConfiguration> options,
    IMemoryCache cache
) : IMiddleware
{
    [LoggerMessage(Level = LogLevel.Debug, Message = "Retrieving rate limiter for {Key}")]
    private static partial void LogRateLimitKey(ILogger logger, IPAddress key);

    [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving rate limit for {Name} ({Limit})")]
    private static partial void LogRateLimitForUser(ILogger logger, string name, int limit);

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (IsValidRequest(context) is false)
        {
            await RejectRequest(context);
            return;
        }

        var limiter = GetRateLimiter(context);
        using var lease = await limiter.AcquireAsync(permitCount: 1, context.RequestAborted);
        if (limiter.GetStatistics() is { } statistics)
        {
            context.Response.Headers["X-RateLimit-Limit"] = $"{options.Value.RateLimit}";
            context.Response.Headers["X-RateLimit-Remaining"] = $"{statistics.CurrentAvailablePermits}";
            if (lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                context.Response.Headers["Retry-After"] = $"{retryAfter.Seconds}";
        }

        if (lease.IsAcquired is false)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return;
        }

        await next(context);
    }

    /// <summary>
    /// Checks if the request is "valid" (contains the correct X-Super-* headers).
    /// </summary>
    private bool IsValidRequest(HttpContext context)
    {
        if (options.Value.ValidateClients is false || context.Request.Path.StartsWithSegments("/metrics"))
            return true;

        return context.Request.Headers.ContainsKey(Constants.CLIENT_HEADER_NAME)
            && context.Request.Headers.ContainsKey(Constants.CONTACT_HEADER_NAME);
    }

    private RateLimiter GetRateLimiter(HttpContext http)
    {
        if (http.User.Identity?.IsAuthenticated ?? false)
            return GetRateLimiterForUser(http.User);

        var key = http.Connection.RemoteIpAddress ?? IPAddress.Loopback;
        LogRateLimitKey(logger, key);

        return cache.GetOrCreate(key, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(options.Value.RateLimitWindow);
            return new TokenBucketRateLimiter(new()
            {
                AutoReplenishment = true,
                TokenLimit = options.Value.RateLimit,
                TokensPerPeriod = options.Value.RateLimit,
                QueueLimit = 0,
                ReplenishmentPeriod = TimeSpan.FromSeconds(options.Value.RateLimitWindow)
            });
        }) ?? throw new InvalidOperationException($"Creating rate limiter failed for {key}");
    }

    private RateLimiter GetRateLimiterForUser(ClaimsPrincipal user)
    {
        var name = user.Identity?.Name!;
        var limit = user.GetIntClaim("RateLimit");

        LogRateLimitForUser(logger, name, limit);
        return cache.GetOrCreate(name, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(options.Value.RateLimitWindow);
            return new TokenBucketRateLimiter(new()
            {
                AutoReplenishment = true,
                TokenLimit = limit,
                TokensPerPeriod = limit,
                QueueLimit = 0,
                ReplenishmentPeriod = TimeSpan.FromSeconds(options.Value.RateLimitWindow)
            });
        }) ?? throw new InvalidOperationException($"Creating rate limiter failed for {name}");
    }

    private async Task RejectRequest(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.Headers.WWWAuthenticate = "X-Super-Client";
        context.Response.ContentType = "application/json";

        var writer = new Utf8JsonWriter(context.Response.Body);
        writer.WriteStartObject();
        writer.WritePropertyName("message");
        writer.WriteStringValue("The X-Super-Client and X-Super-Contact headers are required");
        writer.WriteEndObject();

        await writer.FlushAsync(context.RequestAborted);
    }
}
