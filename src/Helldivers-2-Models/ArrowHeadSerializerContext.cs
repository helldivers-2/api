using Helldivers.Models.ArrowHead;
using System.Text.Json.Serialization;

namespace Helldivers.Models;

/// <summary>
/// Source generated <see cref="JsonSerializerContext" /> for ArrowHead models.
/// </summary>
[JsonSerializable(typeof(WarId))]
[JsonSerializable(typeof(WarInfo))]
[JsonSerializable(typeof(WarStatus))]
[JsonSerializable(typeof(NewsFeedItem))]
[JsonSerializable(typeof(WarSummary))]
[JsonSerializable(typeof(List<Assignment>))]
[JsonSerializable(typeof(List<NewsFeedItem>))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]
public sealed partial class ArrowHeadSerializerContext : JsonSerializerContext
{

}
