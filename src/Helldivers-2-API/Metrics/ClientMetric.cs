using System.Text.RegularExpressions;

namespace Helldivers.API.Metrics;

/// <summary>
/// Handles the logic for generating the `Client` label in the Prometheus metrics for HTTP requests.
/// </summary>
public static partial class ClientMetric
{
    /// <summary>
    /// The name of a client that couldn't be identified.
    /// </summary>
    private const string UNKNOWN_CLIENT_NAME = "Unknown";

    /// <summary>
    /// The name for clients that are developing locally (e.g. localhost, ...).
    /// </summary>
    private const string LOCAL_DEVELOPMENT_CLIENT_NAME = "Development";

    [GeneratedRegex(@"^[a-z0-9]{24}--.+\.netlify\.app$")]
    private static partial Regex IsNetlifyApp();

    /// <summary>
    /// Extracts the name of the client for the incoming request.
    /// </summary>
    public static string GetClientName(HttpContext context)
    {
        // If we have an authenticated user, use their name instead
        if (context.User.Identity is { Name: { } name })
            return name;

        // If the client sends `X-Super-Client` we use that name
        if (context.Request.Headers.TryGetValue("X-Super-Client", out var superClient))
            if (string.IsNullOrWhiteSpace(superClient) is false)
                return superClient!;

        if (GetBrowserClientName(context.Request) is { } clientName)
            return clientName;

        return UNKNOWN_CLIENT_NAME;
    }

    /// <summary>
    /// If the client is a browser it sends specific headers along.
    /// Attempt to parse out those headers to get the domain and perform some additional normalization.
    ///
    /// If we can't find any of those headers simply return <c>null</c>
    /// </summary>
    private static string? GetBrowserClientName(HttpRequest request)
    {
        string? result = null;
        if (string.IsNullOrWhiteSpace(request.Headers.Referer) is false)
            result = request.Headers.Referer!;
        if (string.IsNullOrWhiteSpace(request.Headers.Origin) is false)
            result = request.Headers.Origin!;

        if (result is not null && Uri.TryCreate(result, UriKind.Absolute, out var uri))
        {
            // Localhost etc just return local development for a client name.
            if (uri.Host.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
                return LOCAL_DEVELOPMENT_CLIENT_NAME;

            // Netlify seems to generate urls with random prefixes like following:
            // <24 characters random>--<projectname>.netlify.app
            // we normalize to <projectname>.netlify.app
            if (IsNetlifyApp().IsMatch(uri.Host))
                return uri.Host[26..];

            return uri.Host;
        }

        return result;
    }
}
