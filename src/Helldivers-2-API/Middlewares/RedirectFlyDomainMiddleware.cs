namespace Helldivers.API.Middlewares;

/// <summary>
/// Automatically generates a redirect if a user still uses the deprecated `fly.io` URL.
/// </summary>
public class RedirectFlyDomainMiddleware : IMiddleware
{
    private const string RedirectDomain = "https://api.helldivers2.dev";

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Host.Host.Equals("helldivers-2-dotnet.fly.dev", StringComparison.InvariantCultureIgnoreCase))
        {
            var url = $"{RedirectDomain}{context.Request.Path}";

            context.Response.Redirect(url, permanent: true);
        }

        await next(context);
    }
}
