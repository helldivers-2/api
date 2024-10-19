﻿using Helldivers.Models;
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
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Fetch the newsfeed of a given <paramref name="season" /> in <paramref name="language" />.
    /// </summary>
    public async Task<Memory<byte>> LoadFeed(string season, string language, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/NewsFeed/{season}?maxEntries=1024", language);
        using var response = await http.SendAsync(request, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        return await CollectStream(stream, cancellationToken);
    }

    /// <summary>
    /// Loads assignments of a given <paramref name="season" /> in <paramref name="language" />.
    /// </summary>
    public async Task<Memory<byte>> LoadAssignments(string season, string language, CancellationToken cancellationToken)
    {
        var request = BuildRequest($"/api/v2/Assignment/War/{season}", language);
        using var response = await http.SendAsync(request, cancellationToken);
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
