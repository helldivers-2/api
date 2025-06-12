using Helldivers.Models.V1;
using Helldivers.Models.V1.Planets;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Source generated <see cref="JsonSerializerContext" /> for V1's models.
/// </summary>
[JsonSerializable(typeof(Assignment))]
[JsonSerializable(typeof(List<Assignment>))]
[JsonSerializable(typeof(Campaign))]
[JsonSerializable(typeof(List<Campaign>))]
[JsonSerializable(typeof(Dispatch))]
[JsonSerializable(typeof(List<Dispatch>))]
[JsonSerializable(typeof(Planet))]
[JsonSerializable(typeof(List<Planet>))]
[JsonSerializable(typeof(Statistics))]
[JsonSerializable(typeof(SteamNews))]
[JsonSerializable(typeof(List<SteamNews>))]
[JsonSerializable(typeof(War))]
[JsonSerializable(typeof(Region))]
[JsonSerializable(typeof(List<Region>))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, UseStringEnumConverter = true)]
public sealed partial class V1SerializerContext : JsonSerializerContext
{

}
