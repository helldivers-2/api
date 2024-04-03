using Helldivers.Models.Steam;
using Helldivers.Models.V1;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Source generated <see cref="JsonSerializerContext" /> for Steam models.
/// </summary>
[JsonSerializable(typeof(SteamNewsFeed))]
[JsonSerializable(typeof(List<SteamNews>))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public sealed partial class SteamSerializerContext : JsonSerializerContext
{
    //
}
