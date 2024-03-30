using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Threading.RateLimiting;

namespace Helldivers.API.Middlewares;

/// <summary>
/// Handles applying rate limit logic to the API's requests.
/// </summary>
public sealed partial class RateLimitMiddleware(ILogger<RateLimitMiddleware> logger, IMemoryCache cache) : IMiddleware
{
    private const int DefaultRequestLimit = 5;
    private const int DefaultRequestWindow = 10;

    [LoggerMessage(Level = LogLevel.Information, Message = "Retrieving rate limiter for {Key}")]
    private static partial void LogRateLimitKey(ILogger logger, IPAddress key);

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
}
