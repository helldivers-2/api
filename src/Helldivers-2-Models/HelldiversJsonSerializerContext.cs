using Helldivers.Models.ArrowHead;
using Helldivers.Models.Domain;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Contains all source generated type information for classes in the Models library.
/// </summary>
[JsonSerializable(typeof(WarId))]
[JsonSerializable(typeof(WarInfo))]
[JsonSerializable(typeof(WarStatus))]
[JsonSerializable(typeof(NewsFeedItem))]
[JsonSerializable(typeof(Assignment))]
[JsonSerializable(typeof(GalacticWar))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public sealed partial class HelldiversJsonSerializerContext : JsonSerializerContext
{
}
