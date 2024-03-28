using Helldivers.Models.ArrowHead;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Contains all source generated type information for classes in the Models library.
/// </summary>
[JsonSerializable(typeof(WarInfo))]
[JsonSerializable(typeof(WarStatus))]
[JsonSerializable(typeof(NewsFeedItem))]
[JsonSerializable(typeof(Assignment))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public sealed partial class HelldiversJsonSerializerContext : JsonSerializerContext
{
}
