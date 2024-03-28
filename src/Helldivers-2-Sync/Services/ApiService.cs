using Helldivers.Models;
using Helldivers.Models.ArrowHead;
using Helldivers.Sync.Configuration;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Helldivers.Sync.Services;

/// <summary>
/// Handles communication with the ArrowHead API.
/// </summary>
public sealed class ApiService(
    IOptions<HelldiversSyncConfiguration> options,
    HttpClient http
)
{
    /// <summary>
    /// Gets the identifier of the current war season from ArrowHead's API.
    /// </summary>
    public async Task<string> GetCurrentSeason(CancellationToken cancellationToken)
    {
        var request = BuildRequest("/api/WarSeason/current/WarID");
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var warId = await JsonSerializer
                        .DeserializeAsync(stream, HelldiversJsonSerializerContext.Default.WarId, cancellationToken)
                    ?? throw new InvalidOperationException();

        return warId.Id.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Fetch <see cref="WarInfo" /> from ArrowHead's API.
    /// </summary>
    public async Task<WarInfo> GetWarInfo(string season, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/WarSeason/{season}/WarInfo");
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer
                   .DeserializeAsync(stream, HelldiversJsonSerializerContext.Default.WarInfo, cancellationToken)
               ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Fetch <see cref="WarStatus" /> from ArrowHead's API.
    /// </summary>
    public async Task<WarStatus> GetWarStatus(string season, string language, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/WarSeason/{season}/Status", language);
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer
                   .DeserializeAsync(stream, HelldiversJsonSerializerContext.Default.WarStatus, cancellationToken)
               ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Fetch the newsfeed of a given <paramref name="season" /> in <paramref name="language" />.
    /// </summary>
    public async IAsyncEnumerable<NewsFeedItem> LoadFeed(string season, string language,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/NewsFeed/{season}", language);
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var items = JsonSerializer.DeserializeAsyncEnumerable(
            stream,
            HelldiversJsonSerializerContext.Default.NewsFeedItem,
            cancellationToken
        );

        // Unfortunately C# doesn't allow directly returning IAsyncEnumerable.
        await foreach (var item in items)
            yield return item ?? throw new InvalidOperationException("Failed to deserialize newsfeed item");
    }

    /// <summary>
    /// Loads assignments of a given <paramref name="season" /> in <paramref name="language" />.
    /// </summary>
    public async IAsyncEnumerable<Assignment> LoadAssignments(string season, string language,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/v2/Assignment/War/{season}", language);
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var assignments = JsonSerializer.DeserializeAsyncEnumerable(
            stream,
            HelldiversJsonSerializerContext.Default.Assignment,
            cancellationToken
        );

        // Unfortunately C# doesn't allow directly returning IAsyncEnumerable.
        await foreach (var assignment in assignments)
            yield return assignment ?? throw new InvalidOperationException("Failed to deserialize assignment");
    }

    /// <summary>
    /// Fetch <see cref="WarSummary" /> from ArrowHead's API.
    /// </summary>
    public async Task<WarSummary> GetSummary(string season, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/Stats/war/{season}/summary");
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await JsonSerializer
                   .DeserializeAsync(stream, HelldiversJsonSerializerContext.Default.WarSummary, cancellationToken)
               ?? throw new InvalidOperationException();
    }

    private HttpRequestMessage BuildRequest(string url, string? language = null)
    {
        if (string.IsNullOrWhiteSpace(language))
            language = options.Value.Languages.First();

        return new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers = { AcceptLanguage = { StringWithQualityHeaderValue.Parse(language) } }
        };
    }
}
