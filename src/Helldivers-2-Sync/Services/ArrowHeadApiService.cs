using Helldivers.Models;
using Helldivers.Models.ArrowHead;
using Helldivers.Sync.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Helldivers.Sync.Services;

/// <summary>
/// Handles communication with the ArrowHead API.
/// </summary>
public sealed class ArrowHeadApiService(
    IOptions<HelldiversSyncConfiguration> options,
    HttpClient http
)
{
    /// <summary>
    /// Gets the identifier of the current war season from ArrowHead's API.
    /// </summary>
    public async Task<(Memory<byte>, WarId)> GetCurrentSeason(CancellationToken cancellationToken)
    {
        var request = BuildRequest("/api/WarSeason/current/WarID");
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var memory = await CollectStream(stream, cancellationToken);
        var warId = JsonSerializer.Deserialize(
            memory.Span,
            ArrowHeadSerializerContext.Default.WarId
        ) ?? throw new InvalidOperationException();

        return (memory, warId);
    }

    /// <summary>
    /// Fetch <see cref="WarInfo" /> from ArrowHead's API.
    /// </summary>
    public async Task<Memory<byte>> GetWarInfo(string season, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/WarSeason/{season}/WarInfo");
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Fetch <see cref="WarStatus" /> from ArrowHead's API.
    /// </summary>
    public async Task<Memory<byte>> GetWarStatus(string season, string language, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/WarSeason/{season}/Status", language);
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Fetch a single page of the newsfeed of a given <paramref name="season" /> in <paramref name="language" />,
    /// starting at <paramref name="fromTimestamp" />. ArrowHead returns at most <see cref="HelldiversSyncConfiguration.NewsFeedMaxEntries" />
    /// entries per call, oldest-first from <paramref name="fromTimestamp" /> onwards.
    /// </summary>
    private async Task<Memory<byte>> LoadFeedPage(string season, string language, uint fromTimestamp,
        CancellationToken cancellationToken)
    {
        // If the `NewsFeedMaxEntries` flag is not set to 0 we pass it in.
        // This parameter needs to be passed or a 400 status code will be returned occasionally.
        var request = options.Value.NewsFeedMaxEntries is 0
            ? BuildRequest($"/api/NewsFeed/{season}?fromTimestamp={fromTimestamp}", language)
            : BuildRequest($"/api/NewsFeed/{season}?maxEntries={options.Value.NewsFeedMaxEntries}&fromTimestamp={fromTimestamp}", language);

        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Fetch the entire newsfeed of a given <paramref name="season" /> in <paramref name="language" />, paging
    /// through ArrowHead's API (which only ever returns up to <see cref="HelldiversSyncConfiguration.NewsFeedMaxEntries" />
    /// entries per call starting at a given timestamp) until every entry has been collected.
    /// </summary>
    public async Task<Memory<byte>> LoadFeed(string season, string language, CancellationToken cancellationToken)
    {
        var maxEntries = options.Value.NewsFeedMaxEntries;
        var fromTimestamp = options.Value.NewsFeedFromTimestamp;
        var items = new List<NewsFeedItem>();

        // Safety net so a misbehaving upstream (eg. always returning a full page) can't loop forever.
        for (var page = 0; page < 1_000; page++)
        {
            var raw = await LoadFeedPage(season, language, fromTimestamp, cancellationToken);
            var pageItems = JsonSerializer.Deserialize(
                raw.Span,
                ArrowHeadSerializerContext.Default.ListNewsFeedItem
            ) ?? [];

            if (pageItems.Count is 0)
                break;

            items.AddRange(pageItems);

            // A page smaller than what we asked for means we've reached the end of the feed.
            if (maxEntries is 0 || pageItems.Count < maxEntries)
                break;

            // Advance the window to just after the newest entry we've seen so far, so the next page
            // picks up where this one left off instead of re-fetching the same entries forever.
            fromTimestamp = checked((uint)(pageItems.Max(item => item.Published) + 1));
        }

        return JsonSerializer.SerializeToUtf8Bytes(items, ArrowHeadSerializerContext.Default.ListNewsFeedItem);
    }

    /// <summary>
    /// Loads assignments of a given <paramref name="season" /> in <paramref name="language" />.
    /// </summary>
    public async Task<Memory<byte>> LoadAssignments(string season, string language, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/v2/Assignment/War/{season}", language);
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Loads space station of a given <paramref name="season" /> and <paramref name="id"/> in <paramref name="language" />.
    /// </summary>
    public async Task<Memory<byte>> LoadSpaceStations(string season, long id, string language,
        CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/SpaceStation/{season}/{id}", language);
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Fetch <see cref="WarSummary" /> from ArrowHead's API.
    /// </summary>
    public async Task<Memory<byte>> GetSummary(string season, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/Stats/war/{season}/summary");
        using var response = await http.SendAsync(request, cancellationToken);

        // Throw on error responses so we don't have to look down the entire serialisation tree.
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
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

    /// <summary>
    /// Read the <paramref name="stream" /> and store it in a local buffer that can be used with zero-copy semantics.
    /// </summary>
    private async Task<Memory<byte>> CollectStream(Stream stream, CancellationToken cancellationToken)
    {
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, cancellationToken);

        return new Memory<byte>(memory.ToArray());
    }
}
