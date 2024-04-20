using Helldivers.API.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace Helldivers.API.Middlewares;

/// <summary>
/// Handles applying rate limit logic to the API's requests.
/// </summary>
public sealed partial class RateLimitMiddleware(ILogger<RateLimitMiddleware> logger, IMemoryCache cache) : IMiddleware
{
    // TODO: move to configurable policies.
    private const int DefaultRequestLimit = 5;
    private const int DefaultRequestWindow = 10;

    [LoggerMessage(Level = LogLevel.Debug, Message = "Retrieving rate limiter for {Key}")]
    private static partial void LogRateLimitKey(ILogger logger, IPAddress key);

    [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving rate limit for {Name} ({Limit})")]
    private static partial void LogRateLimitForUser(ILogger logger, string name, int limit);

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var limiter = GetRateLimiter(context);
        using var lease = await limiter.AcquireAsync(permitCount: 1, context.RequestAborted);
        if (limiter.GetStatistics() is { } statistics)
        {
            context.Response.Headers["X-RateLimit-Limit"] = $"{DefaultRequestLimit}";
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

    private RateLimiter GetRateLimiter(HttpContext http)
    {
        if (http.User.Identity?.IsAuthenticated ?? false)
            return GetRateLimiterForUser(http.User);

        var key = http.Connection.RemoteIpAddress ?? IPAddress.Loopback;
        LogRateLimitKey(logger, key);

        return cache.GetOrCreate(key, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromSeconds(DefaultRequestWindow);
            return new TokenBucketRateLimiter(new()
            {
                AutoReplenishment = true,
                TokenLimit = DefaultRequestLimit,
                TokensPerPeriod = DefaultRequestLimit,
                QueueLimit = 0,
                ReplenishmentPeriod = TimeSpan.FromSeconds(DefaultRequestWindow)
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
            entry.SlidingExpiration = TimeSpan.FromSeconds(DefaultRequestWindow);
            return new TokenBucketRateLimiter(new()
            {
                AutoReplenishment = true,
                TokenLimit = limit,
                TokensPerPeriod = DefaultRequestLimit,
                QueueLimit = 0,
                ReplenishmentPeriod = TimeSpan.FromSeconds(DefaultRequestWindow)
            });
        }) ?? throw new InvalidOperationException($"Creating rate limiter failed for {name}");
    }
}
