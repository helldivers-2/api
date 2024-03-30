using Helldivers.Models.Domain;
using Helldivers.Models.Steam;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

[JsonSerializable(typeof(SteamNewsFeed))]
[JsonSerializable(typeof(List<SteamNews>))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public sealed partial class SteamSerializerContext : JsonSerializerContext
{
    //
}
