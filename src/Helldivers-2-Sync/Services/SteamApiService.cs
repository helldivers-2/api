using Helldivers.Models;
using Helldivers.Models.Steam;
using System.Text.Json;

namespace Helldivers.Sync.Services;

/// <summary>
/// Handles communication with the Steam API for fetching information.
/// </summary>
public sealed class SteamApiService(HttpClient http)
{
    public async Task<SteamNewsFeed> GetLatest(int count = 20)
    {
        var url = $"https://api.steampowered.com/ISteamNews/GetNewsForApp/v2/?appid=553850&count={count}&feeds=steam_community_announcements";
        await using var response = await http.GetStreamAsync(url);

        var feed = await JsonSerializer.DeserializeAsync(
            response,
            SteamSerializerContext.Default.SteamNewsFeed
        );

        return feed!;
    }
}
