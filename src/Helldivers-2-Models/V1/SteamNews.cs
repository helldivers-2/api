namespace Helldivers.Models.V1;

public sealed record SteamNews(
    string Id,
    string Title,
    string Url,
    string Author,
    string Content,
    DateTime PublishedAt
);
