namespace Helldivers.Models.Domain;

public sealed record SteamNews(
    string Title,
    string Url,
    string Author,
    string Content,
    DateTime PublishedAt
);
