using Helldivers.Models.Domain;
using Helldivers.Models.Domain.Localization;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Contains all source generated type information for classes in the Models library.
/// </summary>
[JsonSerializable(typeof(NewsItem))]
[JsonSerializable(typeof(Assignment))]
[JsonSerializable(typeof(GalacticWar))]
[JsonSerializable(typeof(List<NewsItem>))]
[JsonSerializable(typeof(List<Assignment>))]
[JsonSerializable(typeof(LocalizedMessage))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, UseStringEnumConverter = true)]
public sealed partial class HelldiversSerializerContext : JsonSerializerContext
{
}
