using Helldivers.Core.Mapping.Steam;
using Helldivers.Models;
using Helldivers.Models.Steam;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Helldivers.Core.Test.Mapping.Steam;

[TestSubject(typeof(SteamNewsMapper))]
public class SteamNewsMapperTest : IClassFixture<CoreFixture>
{
    public CoreFixture Fixture { get; }
    public SteamNewsFeed Feed { get; }

    public SteamNewsMapperTest(CoreFixture fixture)
    {
        Fixture = fixture;
        using var stream = new FileStream("Resources/GetNewsForApp.json", FileMode.Open);

        Feed = JsonSerializer.Deserialize(stream, SteamSerializerContext.Default.SteamNewsFeed)!;
    }

    [Fact]
    public void CanResolveSteamNewsMapper()
    {
        Fixture.Services.GetRequiredService<SteamNewsMapper>();
    }

    [Theory]
    [InlineData(
        0,
        "5686430301735711456",
        "PATCH 1.000.104",
        "https://steamstore-a.akamaihd.net/news/externalpost/steam_community_announcements/5686430301735711456",
        "Åtnjutslångsamt"
    )]
    [InlineData(
        1,
        "5686430301713851678",
        "PATCH 1.000.103",
        "https://steamstore-a.akamaihd.net/news/externalpost/steam_community_announcements/5686430301713851678",
        "Åtnjutslångsamt"
    )]
    public void CanMapSteamNews(int index, string id, string title, string url, string author)
    {
        var item = Feed.AppNews.NewsItems[index];
        var mapper = Fixture.Services.GetRequiredService<SteamNewsMapper>();

        var mapped = mapper.MapToDomain(item);
        Assert.Equal(id, mapped.Id);
        Assert.Equal(title, mapped.Title);
        Assert.Equal(url, mapped.Url);
        Assert.Equal(author, mapped.Author);
        Assert.NotEmpty(mapped.Content);
    }
}
